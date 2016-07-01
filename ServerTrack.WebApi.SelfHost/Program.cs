using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace ServerTrack.WebApi.SelfHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:8888/";
            using (WebApp.Start<Startup>(new StartOptions(baseAddress)))
            {
                Console.WriteLine("http server listening on 8888.  Press enter to stop.");
                Console.ReadLine();
            }
        }
    }
}
