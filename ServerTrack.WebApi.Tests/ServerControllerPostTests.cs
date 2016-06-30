﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;

namespace ServerTrack.WebApi.Tests
{
    [TestFixture]
    public class ServerControllerPostTests
    {
        [Test]
        public void ServerController_Post_Returns200OnSuccess()
        {
            var serverName = "testServer";
            var serverLoadData = new ServerLoadEntry
            {
                CpuLoad = 1,
                RamLoad = 2
            };

            var controller = new ServerController();

            var response = controller.Post(serverName, serverLoadData);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
    }
}