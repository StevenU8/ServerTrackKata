using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerTrack.WebApi.Models;

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
                    
                }
            });
        }
    }

    public class ServerLoadData
    {

    }
}