using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories;
using ServerTrack.WebApi.Repositories.DataModels;

namespace ServerTrack.WebApi.Tests
{
    [TestFixture]
    public class ServerControllerGetTests
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
        public void ServerController_Get_Returns200OnSucess()
        {
            _serverLoadRepository.ServerRecords.Add(ServerName, new List<ServerLoadData>());
            var response = _controller.Get(ServerName);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ServerController_Get_Returns404WhenServerIsNotFound()
        {
            var response = _controller.Get(ServerName);

            Assert.That((int)response.StatusCode, Is.EqualTo(404));
        }


        [Test]
        public void ServerController_Get_ReturnsAveragesOverLast60Minutes()
        {
            _serverLoadRepository.ServerRecords.Add(ServerName, new List<ServerLoadData>());
            var response = _controller.Get(ServerName);

            var loadAverages = response.Content.ReadAsAsync<LoadAverages>().Result;

            Assert.NotNull(loadAverages);

            var averageLoadsByMinute = loadAverages.AverageLoadsByMinute;
            Assert.That(averageLoadsByMinute.Count, Is.EqualTo(60));
        }

        [Test]
        public void ServerController_Get_ReturnsAveragesOverLast24Hours()
        {
            _serverLoadRepository.ServerRecords.Add(ServerName, new List<ServerLoadData>());
            var response = _controller.Get(ServerName);

            var loadAverages = response.Content.ReadAsAsync<LoadAverages>().Result;

            Assert.NotNull(loadAverages);

            var averageLoadsByHour = loadAverages.AverageLoadsByHour;
            Assert.That(averageLoadsByHour.Count, Is.EqualTo(24));
        }
    }
}
