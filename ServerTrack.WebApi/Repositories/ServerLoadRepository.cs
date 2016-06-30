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
        public ConcurrentDictionary<string, ServerLoadData> ServerLoadData { get; set; }
    }

    public class ServerLoadData
    {

    }
}