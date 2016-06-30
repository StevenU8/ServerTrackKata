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
        private ServerLoadRepository serverLoadRepository;

        public ServerController() : this(new ServerLoadRepository())
        {

        }

        public ServerController(ServerLoadRepository serverLoadRepository)
        {
            this.serverLoadRepository = serverLoadRepository;
        }

        [HttpPost]
        [Route("{serverName}/LoadData")]
        public HttpResponseMessage Post(string serverName, [FromBody]ServerLoadEntry serverLoadEntry)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
