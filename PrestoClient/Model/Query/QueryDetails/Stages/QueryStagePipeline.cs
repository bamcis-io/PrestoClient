using Newtonsoft.Json;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStagePipeline
    {
        public int PipelineId { get; set; }

        public DateTime FirstStartTime { get; set; }

        public DateTime LastStartTime { get; set; }

        public DateTime LastEndTime { get; set; }

        public bool InputPipeline { get; set; }

        public bool OutputPipeline { get; set; }

        public int TotalDrivers { get; set; }

        public int QueuedDrivers { get; set; }

        public int QueuedPartitionedDrivers { get; set; }

        public int RunningDrivers { get; set; }

        public int RunningPartitionedDrivers { get; set; }

        public int BlockedDrivers { get; set; }

        public int CompletedDrivers { get; set; }

        public string MemoryReservation { get; set; }

        public string RevocableMemoryReservation { get; set; }

        public string SystemMemoryReservation { get; set; }

        public QueryStageDistributionDetails QueuedTime { get; set; }

        public QueryStageDistributionDetails ElaspsedTime { get; set; }

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

        public IEnumerable<OperatorStats> OperatorSummaries { get; set; }

        /// <summary>
        /// TODO: Data example unknown
        /// </summary>
        public IEnumerable<object> Drivers { get; set; }
    }
}
