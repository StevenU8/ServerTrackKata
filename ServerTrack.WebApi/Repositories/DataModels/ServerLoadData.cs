using System;
using ServerTrack.WebApi.Utilities;

namespace ServerTrack.WebApi.Repositories.DataModels
{
    public class ServerLoadData
    {
        
        public ServerLoadData(double cpuLoad, double ramLoad)
        {
            CpuLoad = cpuLoad;
            RamLoad = ramLoad;
            RecordedDate = Clock.Now;
        }

        public ServerLoadData()
        {
        }

        public double CpuLoad { get; set; }
        public double RamLoad { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}