﻿using System;
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
            
            Assert.That(_serverLoadRepository.ServerRecords.Count(), Is.EqualTo(1));

            var savedServerRecords = _serverLoadRepository.ServerRecords.First();
            Assert.That(savedServerRecords.Key, Is.EqualTo(ServerName));

            var serverLoadDataRecords = savedServerRecords.Value;
            Assert.That(serverLoadDataRecords.Count, Is.EqualTo(1));

        }


 
    }
}
