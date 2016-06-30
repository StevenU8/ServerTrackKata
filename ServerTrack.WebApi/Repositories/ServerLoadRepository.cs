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
        public Dictionary<string, ServerLoadData> ServerLoadData { get; set; }

        public ServerLoadRepository()
        {
            ServerLoadData = new Dictionary<string, ServerLoadData>();
        }

        public void Record(ServerLoadEntry serverLoadEntry)
        {
            ServerLoadData.Add("", new ServerLoadData());
        }
    }

    public class ServerLoadData
    {

    }
}