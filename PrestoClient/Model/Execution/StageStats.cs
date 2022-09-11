using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageStats.java
    /// </summary>
    public class StageStats
    {
        #region Public Properties

        public DateTime SchedulingComplete { get; }

        public DistributionSnapshot GetSplitDistribution { get; }

        public int TotalTasks { get; }

        public int RunningTasks { get; }

        public int CompletedTasks { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int RunningDrivers { get; }

        public int BlockedDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeMemory { get; }

        public DataSize TotalMemoryReservation { get; }

        public DataSize PeakMemoryReservation { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalScheduledTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalUserTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalBlockedTime { get; }

        public bool FullyBlocked { get; }

        public HashSet<BlockedReason> BlockedReasons { get; }

        public DataSize RawInputDataSize { get; }

        public long RawInputPositions { get; }

        public DataSize ProcessedInputDataSize { get; }

        public long ProcessedInputPositions { get; }

        public DataSize BufferedDataSize { get; }

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public IEnumerable<OperatorStats> OperatorSummaries { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StageStats(
            DateTime schedulingComplete,
            
            DistributionSnapshot getSplitDistribution,

            int totalTasks,
            int runningTasks,
            int completedTasks,

            int totalDrivers,
            int queuedDrivers,
            int runningDrivers,
            int blockedDrivers,
            int completedDrivers,

            double cumulativeMemory,
            DataSize totalMemoryReservation,
            DataSize peakMemoryReservation,

            TimeSpan totalScheduledTime,
            TimeSpan totalCpuTime,
            TimeSpan totalUserTime,
            TimeSpan totalBlockedTime,
            bool fullyBlocked,
            HashSet<BlockedReason> blockedReasons,

            DataSize rawInputDataSize,
            long rawInputPositions,

            DataSize processedInputDataSize,
            long processedInputPositions,

            DataSize bufferedDataSize,
            DataSize outputDataSize,
            long outputPositions,
            IEnumerable<OperatorStats> operatorSummaries
            )
        {
            this.SchedulingComplete = schedulingComplete;
            this.GetSplitDistribution = getSplitDistribution ?? throw new ArgumentNullException("getSplitDistribution");

            ParameterCheck.OutOfRange(totalTasks >= 0, "totalTasks", "The total tasks cannot less than 0.");
            ParameterCheck.OutOfRange(runningTasks >= 0, "runningTasks", "The running tasks cannot less than 0.");
            ParameterCheck.OutOfRange(completedTasks >= 0, "completedTasks", "The completed tasks cannot less than 0.");
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers", "The total drivers cannot less than 0.");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers", "The queued drivers cannot less than 0.");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers", "The running drivers cannot less than 0.");
            ParameterCheck.OutOfRange(blockedDrivers >= 0, "blockedDrivers", "The blocked drivers cannot less than 0.");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers", "The completed drivers cannot less than 0.");
            ParameterCheck.OutOfRange(cumulativeMemory >= 0 && !Double.IsInfinity(cumulativeMemory) && !Double.IsNaN(cumulativeMemory), "cumulativeMemory", "The value of cumulative memory was invalid.");
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions", "The raw input positions cannot less than 0.");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions", "The processed input positions cannot less than 0.");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions", "The output positions cannot less than 0.");

            this.TotalTasks = totalTasks;
            this.RunningTasks = runningTasks;
            this.CompletedTasks = completedTasks;

            this.TotalDrivers = totalDrivers;
            this.QueuedDrivers = queuedDrivers;
            this.RunningDrivers = runningDrivers;
            this.BlockedDrivers = blockedDrivers;
            this.CompletedDrivers = completedDrivers;

            this.CumulativeMemory = cumulativeMemory;
            this.TotalMemoryReservation = totalMemoryReservation;// ?? throw new ArgumentNullException("totalMemoryReservation");
            this.PeakMemoryReservation = peakMemoryReservation;// ?? throw new ArgumentNullException("peakMemoryReservation");

            this.TotalScheduledTime = totalScheduledTime;
            this.TotalCpuTime = totalCpuTime;
            this.TotalUserTime = totalUserTime;
            this.TotalBlockedTime = totalBlockedTime;
            this.FullyBlocked = fullyBlocked;
            this.BlockedReasons = blockedReasons;

            this.RawInputDataSize = rawInputDataSize ?? throw new ArgumentNullException("rawInputDataSize");
            this.RawInputPositions = rawInputPositions;

            this.ProcessedInputDataSize = processedInputDataSize ?? throw new ArgumentNullException("processedInputDataSize");
            this.ProcessedInputPositions = processedInputPositions;

            this.BufferedDataSize = bufferedDataSize ?? throw new ArgumentNullException("bufferedDataSize");
            this.OutputDataSize = outputDataSize ?? throw new ArgumentNullException("outputDataSize");
            this.OutputPositions = outputPositions;
            this.OperatorSummaries = operatorSummaries ?? throw new ArgumentNullException("operatorSummaries");
        }

        #endregion
    }
}
