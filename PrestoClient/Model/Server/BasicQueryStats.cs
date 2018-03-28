using Newtonsoft.Json;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using BAMCIS.PrestoClient.Model.Execution.Scheduler;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// Lightweight version of QueryStats. Parts of the web UI depend on the fields
    /// being named consistently across these classes.
    /// 
    /// From com.facebook.presto.server.BasicQueryStats.java
    /// </summary>
    public class BasicQueryStats
    {
        public DateTime CreateTime { get; set; }

        public DateTime EndTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExecutionTime { get; set; }

        public int TotalDrivers { get; set; }

        public int QueuedDrivers { get; set; }

        public int RunningDrivers { get; set; }

        public int CompletedDrivers { get; set; }

        public double CumulativeMemory { get; set; }

        public string UserMemoryReservation { get; set; }

        public string PeakMemoryReservation { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; set; }

        public bool FullyBlocked { get; set; }

        public HashSet<BlockedReason> BlockedReasons { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage { get; set; }
    }
}