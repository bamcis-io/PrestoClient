using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.QueryStats.java
    /// </summary>
    public class QueryStats
    {
        public DateTime CreateTime { get; set; }

        public DateTime ExecutionStartTime { get; set; }

        public DateTime LastHeartBeat { get; set; }

        public DateTime EndTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan ElapsedTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan QueuedTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan AnalysisTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan DistributedPlanningTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalPlanningTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan FinishingTime { get; set; }

        public int TotalTasks { get; set; }

        public int RunningTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int TotalDrivers { get; set; }

        public int QueuedDrivers { get; set; }

        public int RunningDrivers { get; set; }

        public int BlockedDrivers { get; set; }

        public int CompletedDrivers { get; set; }

        public double CumulativeMemory { get; set; }

        public string UserMemoryReservation { get; set; }

        public string PeakUserMemoryReservation { get; set; }

        public string PeakTotalMemoryReservation { get; set; }

        public bool Scheduled { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalScheduledTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalCpuTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalUserTime { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan TotalBlockedTime { get; set; }

        public bool FullyBlocked { get; set; }

        public HashSet<BlockedReason> BlockedReasons { get; set; }

        public string RawInputDataSize { get; set; }

        public long RawInputPositions { get; set; }

        public string ProcessedInputDataSize { get; set; }

        public long ProcessedInputPositions { get; set; }

        public string OutputDataSize { get; set; }

        public long OutputPositions { get; set; }

        public string PhysicalWrittenDataSize { get; set; }

        public IEnumerable<OperatorStats> OperatorSummaries { get; set; }

        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Calculated property on the server side
        /// </summary>
        public string LogicalWrittenDataSize { get; set; }

        /// <summary>
        /// Calculated property on the server side
        /// </summary>
        public long WrittenPositions { get; set; }

        /// <summary>
        /// Calculated property on the server side
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage { get; set; }
    }
}