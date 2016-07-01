using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories;

namespace ServerTrack.WebApi.Controllers
{
    [RoutePrefix("api/Server")]
    public class ServerController : ApiController
    {
        private readonly ServerLoadRepository _serverLoadRepository;

        public ServerController(ServerLoadRepository serverLoadRepository)
        {
            _serverLoadRepository = serverLoadRepository;
        }

        [HttpPost]
        [Route("{serverName}/LoadData")]
        public async Task<HttpResponseMessage> Post(string serverName, [FromBody]ServerLoadEntry serverLoadEntry)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            await Task.Run(() => _serverLoadRepository.Record(serverName, serverLoadEntry));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{serverName}/LoadData")]
        public HttpResponseMessage Get(string serverName)
        {
            var lastHourAverages = _serverLoadRepository.GetAverageLoads(serverName, LoadAverage.Minutes);

            if (lastHourAverages == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var lastDayAverages = _serverLoadRepository.GetAverageLoads(serverName, LoadAverage.Hours);
            var loadAverages = new LoadAverages
            {
                AverageLoadsByHour = lastDayAverages,
                AverageLoadsByMinute = lastHourAverages
            };

            var content = new ObjectContent<LoadAverages>(loadAverages, new JsonMediaTypeFormatter(), "application/json");
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

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
        public DateTime RangeStart { get; set; }
        public DateTime RangeEnd { get; set; }
        public double AverageCpuLoad { get; set; }
        public double AverageRamLoad { get; set; }
    }

    public enum LoadAverage
    {
        Minutes = 60,
        Hours = 24
    }
}

