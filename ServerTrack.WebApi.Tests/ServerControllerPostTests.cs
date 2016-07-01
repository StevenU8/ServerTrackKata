using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ServerTrack.WebApi.Controllers;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.Repositories;
using ServerTrack.WebApi.Repositories.DataModels;
using ServerTrack.WebApi.Utilities;

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

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ServerController_Post_InvalidServerNameReturns400(string serverName)
        {
            var serverLoadData = new ServerLoadEntry();

            var response = _controller.Post(serverName, serverLoadData);

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void ServerController_Post_SavesNewServerLoadDataEntry()
        {
            var expectedRecordedDate = DateTime.Now;
            Clock.Freeze(expectedRecordedDate);

            var serverLoadData = new ServerLoadEntry
            {
                CpuLoad = 1.00d,
                RamLoad = 2.00d
            };

            _controller.Post(ServerName, serverLoadData);

            Assert.That(_serverLoadRepository.ServerRecords.Count(), Is.EqualTo(1));

            var savedServerRecords = _serverLoadRepository.ServerRecords.Single();
            Assert.That(savedServerRecords.Key, Is.EqualTo(ServerName));

            var serverLoadDataRecords = savedServerRecords.Value;
            Assert.That(serverLoadDataRecords.Count, Is.EqualTo(1));

            var serverLoadDataRecord = serverLoadDataRecords.Single();

            Assert.That(serverLoadDataRecord.CpuLoad, Is.EqualTo(1.00d));
            Assert.That(serverLoadDataRecord.RamLoad, Is.EqualTo(2.00d));
            Assert.That(serverLoadDataRecord.RecordedDate, Is.EqualTo(expectedRecordedDate));
            
        }

        [Test]
        public void ServerController_Post_AppendsLoadDataEntry()
        {
            var expectedRecordedDate = DateTime.Now;
            Clock.Freeze(expectedRecordedDate);

            _serverLoadRepository.ServerRecords.TryAdd(ServerName, new BlockingCollection<ServerLoadData>
            {
                new ServerLoadData
                {
                    CpuLoad = 100d,
                    RamLoad = 200d,
                    RecordedDate = expectedRecordedDate
                }
            });

            var serverLoadData = new ServerLoadEntry
            {
                CpuLoad = 1.00d,
                RamLoad = 2.00d
            };

            _controller.Post(ServerName, serverLoadData);

            Assert.That(_serverLoadRepository.ServerRecords.Count(), Is.EqualTo(1));

            var savedServerRecords = _serverLoadRepository.ServerRecords.Single();
            Assert.That(savedServerRecords.Key, Is.EqualTo(ServerName));

            var serverLoadDataRecords = savedServerRecords.Value.ToList();
            Assert.That(serverLoadDataRecords.Count, Is.EqualTo(2));

            Assert.That(serverLoadDataRecords[0].CpuLoad, Is.EqualTo(100d));
            Assert.That(serverLoadDataRecords[0].RamLoad, Is.EqualTo(200d));
            Assert.That(serverLoadDataRecords[0].RecordedDate, Is.EqualTo(expectedRecordedDate));

            Assert.That(serverLoadDataRecords[1].CpuLoad, Is.EqualTo(1.00d));
            Assert.That(serverLoadDataRecords[1].RamLoad, Is.EqualTo(2.00d));
            Assert.That(serverLoadDataRecords[1].RecordedDate, Is.EqualTo(expectedRecordedDate));

        }
    }
}
