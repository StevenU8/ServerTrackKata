using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using ServerTrack.WebApi.Models;
using ServerTrack.WebApi.SelfHost;

namespace ServerTrack.WebApi.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void LoadTest_1000Records()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            const string baseUrl = "http://localhost:8888/";
            const string url = baseUrl + "api/server/test/loaddata";

            using (WebApp.Start<Startup>(baseUrl))
            {
                for (var i = 0; i < 1000; i++)
                {
                    var serverLoadEntry = new ServerLoadEntry
                    {
                        CpuLoad = i,
                        RamLoad = i
                    };

                    var content = new ObjectContent<ServerLoadEntry>(serverLoadEntry, new JsonMediaTypeFormatter(), "application/json");

                    var handler = new HttpClientHandler { UseDefaultCredentials = true };
                    using (var httpClient = new HttpClient(handler, true))
                    {
                        httpClient.PostAsync(url, content);
                    }
                }
            }

            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
