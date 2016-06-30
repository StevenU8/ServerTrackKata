using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
