using Newtonsoft.Json;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStageTaskStats
    {
        public DateTime CreateTime { get; set; }

        public DateTime FirstStartTime { get; set; }

        public DateTime LastStartTime { get; set; }

        public DateTime LastEndTime { get; set; }

        public DateTime EndTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan ElapsedTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan QueuedTime { get; set; }

        public int TotalDrivers { get; set; }

        public int QueuedDrivers { get; set; }

        public int QueuedPartitionedDrivers { get; set; }

        public int RunningDrivers { get; set; }

        public int RunningPartitionedDrivers { get; set; }

        public int BlockedDrivers { get; set; }

        public int CompletedDrivers { get; set; }

        public double CumulativeMemory { get; set; }

        public string MemoryReservation { get; set; }

        public string RevocableMemoryReservation { get; set; }

        public string SystemMemoryReservation { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalScheduledTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalCpuTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalUserTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalBlockedTime { get; set; }

        public bool FullyBlocked { get; set; }

        /// <summary>
        /// TODO: Data example unknown
        /// </summary>
        public IEnumerable<string> BlockedReasons { get; set; }

        public string RawInputDataSize { get; set; }

        public int RawInputPositions { get; set; }

        public string ProcessedInputDataSize { get; set; }

        public int ProcessedInputPositions { get; set; }

        public string OutputDataSize { get; set; }

        public int OutputPositions { get; set; }

        public string PhysicalWrittenDataSize { get; set; }

        public IEnumerable<QueryStagePipeline> Pipelines { get; set; }
    }
}
