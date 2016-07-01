using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories.DataModels;
using ServerTrack.WebApi.Utilities;

namespace ServerTrack.WebApi.Repositories
{
    public class ServerLoadRepository
    {
        public ConcurrentDictionary<string, BlockingCollection<ServerLoadData>> ServerRecords { get; set; }

        public ServerLoadRepository()
        {
            ServerRecords = new ConcurrentDictionary<string, BlockingCollection<ServerLoadData>>();
        }

        public void Record(string serverName, ServerLoadEntry serverLoadEntry)
        {
            ServerRecords.AddOrUpdate(serverName,
                s => new BlockingCollection<ServerLoadData> { new ServerLoadData(serverLoadEntry) },
                (s, list) =>
                {
                    list.Add(new ServerLoadData(serverLoadEntry));
                    return list;
                });
        }

        public List<AverageLoad> GetAverageLoads(string serverName, LoadAverage loadAverageType)
        {
            if (!ServerRecords.ContainsKey(serverName))
                return null;

            var serverLoadDataRecords = ServerRecords[serverName];

            var currentTime = Clock.Now;
            var timeframeStart = currentTime.AddSeconds(-currentTime.Second);
            var averageLoads = new List<AverageLoad>();
            for (var i = 1; i < (int)loadAverageType + 1; i++)
            {
                var timeframeEnd = timeframeStart.AddSeconds(-1);
                timeframeStart = GetUpdatedTimeFrameStart(loadAverageType, timeframeStart);

                var relevantRecordsInTimeFrame = serverLoadDataRecords
                    .Where(l => l.RecordedDate >= timeframeStart && l.RecordedDate <= timeframeEnd)
                    .ToList();

                var relevantCpuLoads = relevantRecordsInTimeFrame
                    .Select(r => r.CpuLoad)
                    .ToList();

                var relevantRamLoads = relevantRecordsInTimeFrame
                    .Select(r => r.RamLoad)
                    .ToList();

                averageLoads.Add(new AverageLoad
                {
                    RangeStart = timeframeStart,
                    RangeEnd = timeframeEnd,
                    AverageCpuLoad = relevantCpuLoads.Any() ? relevantCpuLoads.Average() : 0,
                    AverageRamLoad = relevantRamLoads.Any() ? relevantRamLoads.Average() : 0
                });
            }
            return averageLoads;
        }

        private static DateTime GetUpdatedTimeFrameStart(LoadAverage loadAverageType, DateTime timeframeStart)
        {
            switch (loadAverageType)
            {
                case (LoadAverage.Minutes):
                    return timeframeStart.AddMinutes(-1);
                case (LoadAverage.Hours):
                    return timeframeStart.AddHours(-1);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}