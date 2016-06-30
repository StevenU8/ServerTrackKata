using System;
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
        private ServerController _controller;
        private const string ServerName = "testServer";

        [SetUp]
        public void BeforeEachTest()
        {
            _controller = new ServerController();
        }

        [Test]
        public void ServerController_Post_Returns200OnSuccess()
        {
            var serverLoadData = new ServerLoadEntry();

            var response = _controller.Post(ServerName, serverLoadData);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ServerController_Post_SavesNewServerLoadDataEntry()
        {
            
        }
    }
}
