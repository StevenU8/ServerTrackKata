using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerTrack.WebApi.Utilities
{
    public static class Clock
    {
        private static bool Frozen { get; set; }
        private static DateTime FreezeTime { get; set; }

        /// <summary>
        /// The current time
        /// </summary>
        public static DateTime Now
        {
            get
            {
                var now = DateTime.Now;
                if (Frozen)
                {
                    now = FreezeTime;
                }

                return now;
            }
        }


        /// <summary>
        /// Freeze the clock at a specific time
        /// </summary>
        public static void Freeze(DateTime freezeTime)
        {
            FreezeTime = freezeTime;
            Frozen = true;
        }
    }
}