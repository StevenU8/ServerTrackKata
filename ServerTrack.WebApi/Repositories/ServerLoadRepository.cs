using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}