using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories;

namespace ServerTrack.WebApi.Controllers
{
    [RoutePrefix("api/Server")]
    public class ServerController : ApiController
    {
        private ServerLoadRepository _serverLoadRepository;

        public ServerController() : this(new ServerLoadRepository())
        {
            
        }

        public ServerController(ServerLoadRepository serverLoadRepository)
        {
            this._serverLoadRepository = serverLoadRepository;
        }

        [HttpPost]
        [Route("{serverName}/LoadData")]
        public HttpResponseMessage Post(string serverName, [FromBody]ServerLoadEntry serverLoadEntry)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _serverLoadRepository.Record(serverName, serverLoadEntry);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage Get(string serverName)
        {
            var lastHourAverages = _serverLoadRepository.GetAverageLoads(serverName, LoadAverage.Minutes, 60);

            if (lastHourAverages == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var lastDayAverages = _serverLoadRepository.GetAverageLoads(serverName, LoadAverage.Hours, 24);

            var loadAverages = new LoadAverages
            {
                AverageLoadsByHour = lastDayAverages,
                AverageLoadsByMinute = lastHourAverages
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ObjectContent<LoadAverages>(loadAverages,new JsonMediaTypeFormatter(),  "application/json");

            return response;
        }
    }

    public class LoadAverages
    {
        public List<AverageLoad> AverageLoadsByMinute { get; set; }
        public List<AverageLoad> AverageLoadsByHour { get; set; }
    }

    public class AverageLoad    
    {
    }

    public enum LoadAverage
    {
        Minutes,
        Hours
    }
}

