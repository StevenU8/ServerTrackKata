using System;
using System.Collections.Concurrent;
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
using ServerTrack.WebApi.Utilities;

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
            _serverLoadRepository.ServerRecords.TryAdd(ServerName, new BlockingCollection<ServerLoadData>());
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
            var currentTime = new DateTime(2016, 6, 30, 2, 0, 0);
            Clock.Freeze(currentTime);
            var serverLoadDatas = new BlockingCollection<ServerLoadData>
            {
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddMinutes(-1),
                    CpuLoad = 1,
                    RamLoad = 2,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddMinutes(-1),
                    CpuLoad = 3,
                    RamLoad = 4,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddMinutes(-40),
                    CpuLoad = 5,
                    RamLoad = 6,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddMinutes(-40),
                    CpuLoad = 7,
                    RamLoad = 8,
                },
            };
            _serverLoadRepository.ServerRecords.TryAdd(ServerName, serverLoadDatas);
            var response = _controller.Get(ServerName);

            var loadAverages = response.Content.ReadAsAsync<LoadAverages>().Result;

            Assert.NotNull(loadAverages);

            var averageLoadsByMinute = loadAverages.AverageLoadsByMinute;
            Assert.That(averageLoadsByMinute.Count, Is.EqualTo(60));
            Assert.That(averageLoadsByMinute[0].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 1, 59, 0)));
            Assert.That(averageLoadsByMinute[0].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 1, 59, 59)));
            Assert.That(averageLoadsByMinute[0].AverageCpuLoad, Is.EqualTo(2));
            Assert.That(averageLoadsByMinute[0].AverageRamLoad, Is.EqualTo(3));
            
            Assert.That(averageLoadsByMinute[1].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 1, 58, 0)));
            Assert.That(averageLoadsByMinute[1].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 1, 58, 59)));
            Assert.That(averageLoadsByMinute[1].AverageCpuLoad, Is.EqualTo(0));
            Assert.That(averageLoadsByMinute[1].AverageRamLoad, Is.EqualTo(0));

            Assert.That(averageLoadsByMinute[39].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 1, 20, 0)));
            Assert.That(averageLoadsByMinute[39].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 1, 20, 59)));
            Assert.That(averageLoadsByMinute[39].AverageCpuLoad, Is.EqualTo(6));
            Assert.That(averageLoadsByMinute[39].AverageRamLoad, Is.EqualTo(7));
        }

        [Test]
        public void ServerController_Get_ReturnsAveragesOverLast24Hours()
        {
            var currentTime = new DateTime(2016, 6, 30, 18, 0, 0);
            Clock.Freeze(currentTime);
            var serverLoadDatas = new BlockingCollection<ServerLoadData>
            {
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddHours(-1),
                    CpuLoad = 1,
                    RamLoad = 2,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddHours(-1),
                    CpuLoad = 3,
                    RamLoad = 4,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddHours(-12),
                    CpuLoad = 5,
                    RamLoad = 6,
                },
                new ServerLoadData
                {
                    RecordedDate = currentTime.AddHours(-12),
                    CpuLoad = 7,
                    RamLoad = 8,
                },
            };
            _serverLoadRepository.ServerRecords.TryAdd(ServerName, serverLoadDatas);
            var response = _controller.Get(ServerName);

            var loadAverages = response.Content.ReadAsAsync<LoadAverages>().Result;

            Assert.NotNull(loadAverages);

            var averageLoadsByHour = loadAverages.AverageLoadsByHour;
            Assert.That(averageLoadsByHour.Count, Is.EqualTo(24));
            Assert.That(averageLoadsByHour[0].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 17, 0, 0)));
            Assert.That(averageLoadsByHour[0].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 17, 59, 59)));
            Assert.That(averageLoadsByHour[0].AverageCpuLoad, Is.EqualTo(2));
            Assert.That(averageLoadsByHour[0].AverageRamLoad, Is.EqualTo(3));

            Assert.That(averageLoadsByHour[1].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 16, 0, 0)));
            Assert.That(averageLoadsByHour[1].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 16, 59, 59)));
            Assert.That(averageLoadsByHour[1].AverageCpuLoad, Is.EqualTo(0));
            Assert.That(averageLoadsByHour[1].AverageRamLoad, Is.EqualTo(0));

            Assert.That(averageLoadsByHour[11].RangeStart, Is.EqualTo(new DateTime(2016, 6, 30, 6, 00, 0)));
            Assert.That(averageLoadsByHour[11].RangeEnd, Is.EqualTo(new DateTime(2016, 6, 30, 6, 59, 59)));
            Assert.That(averageLoadsByHour[11].AverageCpuLoad, Is.EqualTo(6));
            Assert.That(averageLoadsByHour[11].AverageRamLoad, Is.EqualTo(7));
        }
    }
}
