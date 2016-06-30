using System;

namespace ServerTrack.WebApi.Repositories.DataModels
{
    public class ServerLoadData
    {
        public double CpuLoad { get; set; }
        public double RamLoad { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}