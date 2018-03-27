using Newtonsoft.Json;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStageStatistics
    {
        public DateTime SchedulingComplete { get; set; }

        public QueryStageDistributionDetails GetSplitDistribution { get; set; }

        public QueryStageDistributionDetails ScheduleTaskDistribution { get; set; }

        public QueryStageDistributionDetails AddSplitDistribution { get; set; }

        public int TotalTasks { get; set; }

        public int RunningTasks { get; set; }

        public int CompletedTasks { get; set; }

        public int TotalDrivers { get; set; }

        public int QueuedDrivers { get; set; }

        public int RunningDrivers { get; set; }

        public int BlockedDrivers { get; set; }

        public int CompletedDrivers { get; set; }

        public double CumulativeMemory { get; set; }

        public string TotalMemoryReservation { get; set; }

        public string PeakMemoryReservation { get; set; }

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

        public string BufferedDataSize { get; set; }

        public string OutputDataSize { get; set; }

        public int OutputPositions { get; set; }

        public string PhysicalWrittenDataSize { get; set; }

        public IEnumerable<OperatorStats> OperatorSummaries { get; set; }
    }
}
