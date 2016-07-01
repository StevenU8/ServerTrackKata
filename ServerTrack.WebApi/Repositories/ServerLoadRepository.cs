using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories.DataModels;
using ServerTrack.WebApi.Utilities;

namespace ServerTrack.WebApi.Repositories
{
    public class ServerLoadRepository
    {
        public Dictionary<string, List<ServerLoadData>> ServerRecords { get; set; }

        public ServerLoadRepository()
        {
            ServerRecords = new Dictionary<string, List<ServerLoadData>>();
        }

        public void Record(string serverName, ServerLoadEntry serverLoadEntry)
        {
            List<ServerLoadData> serverLoadDataRecords;
            var recordExists = ServerRecords.TryGetValue(serverName, out serverLoadDataRecords);
            if (recordExists)
            {
                serverLoadDataRecords.Add(new ServerLoadData
                (
                    serverLoadEntry.CpuLoad,
                    serverLoadEntry.RamLoad
                ));
            }
            else
            {
                ServerRecords.Add(serverName, new List<ServerLoadData>
                {
                    new ServerLoadData
                    (
                        serverLoadEntry.CpuLoad, 
                        serverLoadEntry.RamLoad
                    )
                });
            }
        }

        public List<AverageLoad> GetAverageLoads(string serverName, LoadAverage loadAverageType)
        {
            List<ServerLoadData> serverLoadDataRecords;
            var recordExists = ServerRecords.TryGetValue(serverName, out serverLoadDataRecords);
            if (!recordExists)
                return null;

            var currentTime = Clock.Now;
            var timeframeStart = currentTime.AddSeconds(-currentTime.Second);
            var averageLoads = new List<AverageLoad>();
            for (int i = 1; i < (int)loadAverageType + 1; i++)
            {
                var timeframeEnd = timeframeStart.AddSeconds(-1);

                switch (loadAverageType)
                {
                    case (LoadAverage.Minutes):
                        timeframeStart = timeframeStart.AddMinutes(-1);
                        break;
                    case (LoadAverage.Hours):
                        timeframeStart = timeframeStart.AddHours(-1);
                        break;
                    default:
                        throw new Exception();
                }

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
    }

}