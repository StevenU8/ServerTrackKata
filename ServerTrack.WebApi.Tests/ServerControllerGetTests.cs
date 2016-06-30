using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories;

namespace ServerTrack.WebApi.Tests
{
    [TestFixture]
    public class ServerControllerGetTests
    {
        private ServerController _controller;
        private const string ServerName = "testServer";

        [SetUp]
        public void BeforeEachTest()
        {
            _controller = new ServerController();
        }

        [Test]
        public void ServerController_Get_Returns200OnSucess()
        {
            var response = _controller.Get(ServerName);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
        
    }
}
