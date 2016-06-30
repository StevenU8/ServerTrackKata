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
    public class ServerControllerPostTests
    {
        private ServerController _controller;
        private ServerLoadRepository _serverLoadRepository;
        private const string ServerName = "testServer";

        [SetUp]
        public void BeforeEachTest()
        {
            _serverLoadRepository = new ServerLoadRepository();
            _controller = new ServerController(_serverLoadRepository);
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
            var serverLoadData = new ServerLoadEntry();

            _controller.Post(ServerName, serverLoadData);
            
            Assert.That(_serverLoadRepository.ServerLoadData.Count(), Is.EqualTo(1));

            var savedServerLoadDataEntry = _serverLoadRepository.ServerLoadData.First();
            
            Assert.That(savedServerLoadDataEntry.Key, Is.EqualTo(_serverLoadRepository));
        }


 
    }
}
