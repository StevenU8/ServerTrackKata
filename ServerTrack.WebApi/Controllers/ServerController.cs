using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServerTrack.WebApi.Models;

namespace ServerTrack.WebApi.Controllers
{
    [RoutePrefix("api/server")]
    public class ServerController : ApiController
    {
        [HttpPost]
        [Route("{serverName}")]
        public void Post(string serverName, [FromBody]ServerLoadEntry serverLoadEntry)
        {
            
        }
    }
}
