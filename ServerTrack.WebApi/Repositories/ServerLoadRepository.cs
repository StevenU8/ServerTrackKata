using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerTrack.WebApi.Models;
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
            ServerRecords.Add(serverName, new List<ServerLoadData>
            {
                new ServerLoadData
                {
                    CpuLoad = serverLoadEntry.CpuLoad,
                    RamLoad = serverLoadEntry.RamLoad,
                    RecordedDate = Clock.Now
                }
            });
        }
    }

    public class ServerLoadData
    {
        public double CpuLoad { get; set; }
        public double RamLoad { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}