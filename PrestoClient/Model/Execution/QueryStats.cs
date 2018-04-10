using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Model.SPI.EventListener;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.QueryStats.java
    /// </summary>
    public class QueryStats
    {
        #region Public Properties

        public DateTime CreateTime { get; }

        public DateTime ExecutionStartTime { get; }

        public DateTime LastHeartBeat { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan QueuedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AnalysisTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan DistributedPlanningTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalPlanningTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan FinishingTime { get; }

        public int TotalTasks { get; }

        public int RunningTasks { get; }

        public int CompletedTasks { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int RunningDrivers { get; }

        public int BlockedDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeUserMemory { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize PeakUserMemoryReservation { get; }

        public DataSize PeakTotalMemoryReservation { get; }

        public bool Scheduled { get; }

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

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public IEnumerable<StageGcStatistics> StageGcStatistics { get; }

        public IEnumerable<OperatorStats> OperatorSummaries { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExecutionTime { get; }

        public DataSize LogicalWrittenDataSize
        {
            get
            {
                return DataSize.SuccinctBytes(this.OperatorSummaries.Where(x => x.OperatorType != null && x.OperatorType.Equals("tableWriterOperator")).Select(x => x.InputDataSize.ToBytes()).Sum());
            }
        }

        public long WrittenPositions
        {
            get
            {
                return this.OperatorSummaries.Where(x => x.OperatorType != null && x.OperatorType.Equals("tableWriterOperator", StringComparison.OrdinalIgnoreCase)).Select(x => x.InputPositions).Sum();
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage { get
            {
                if (!this.Scheduled || this.TotalDrivers == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Min(100, (this.CompletedDrivers * 100.0) / this.TotalDrivers);
                }
            }
        }

        #endregion

        #region Constructors

        public QueryStats(
            DateTime createTime,
            DateTime executionStartTime,
            DateTime lastHeartBeat,
            DateTime endTime,

            TimeSpan elapsedTime,
            TimeSpan queuedTime,
            TimeSpan analysisTime,
            TimeSpan distributedPlanningTime,
            TimeSpan totalPlanningTime,
            TimeSpan finishingTime,

            int totalTasks,
            int runningTasks,
            int completedTasks,

            int totalDrivers,
            int queuedDrivers,
            int runningDrivers,
            int blockedDrivers,
            int completedDrivers,
            
            double cumulativeUserMemory,
            DataSize userMemoryReservation,
            DataSize peakUserMemoryReservation,
            DataSize peakTotalMemoryReservation,

            bool scheduled,
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

            DataSize outputDataSize,
            long outputPositions,

            DataSize physicalWrittenDataSize,

            IEnumerable<StageGcStatistics> stageGcStatistics,

            IEnumerable<OperatorStats> operatorSummaries
            )
        {
            this.CreateTime = createTime;
            this.ExecutionStartTime = executionStartTime;
            this.LastHeartBeat = lastHeartBeat;
            this.EndTime = endTime;

            this.ElapsedTime = elapsedTime;
            this.QueuedTime = queuedTime;
            this.AnalysisTime = analysisTime;
            this.DistributedPlanningTime = distributedPlanningTime;
            this.TotalPlanningTime = totalPlanningTime;
            this.FinishingTime = finishingTime;

            ParameterCheck.OutOfRange(totalTasks >= 0, "totalTasks", "Total tasks cannot be negative.");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers", "Queued drivers cannot be negative.");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers", "Running drivers cannot be negative.");
            ParameterCheck.OutOfRange(blockedDrivers >= 0, "blockedDrivers", "Blocked drivers cannot be negative.");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers", "Completed drivers cannot be negative.");
            ParameterCheck.OutOfRange(cumulativeUserMemory >= 0, "cumulativeUserMemory", "Cumulative user memory cannot be negative.");
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions", "Raw input positions cannot be negative.");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions", "Processed input positions cannot be negative.");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions", "Output positions cannot be negative.");

            this.TotalTasks = totalTasks;
            this.RunningTasks = runningTasks;
            this.CompletedTasks = completedTasks;

            this.TotalDrivers = totalDrivers;
            this.QueuedDrivers = queuedDrivers;
            this.RunningDrivers = runningDrivers;
            this.BlockedDrivers = blockedDrivers;
            this.CompletedDrivers = completedDrivers;
            this.CumulativeUserMemory = cumulativeUserMemory;
            this.UserMemoryReservation = userMemoryReservation ?? throw new ArgumentNullException("userMemoryReservation");
            this.PeakUserMemoryReservation = peakUserMemoryReservation ?? throw new ArgumentNullException("peakUserMemoryReservation");
            this.PeakTotalMemoryReservation = peakTotalMemoryReservation ?? throw new ArgumentNullException("peakTotalMemoryReservation");
            this.Scheduled = scheduled;
            this.TotalScheduledTime = totalScheduledTime;
            this.TotalCpuTime = totalCpuTime;
            this.TotalBlockedTime = totalBlockedTime;
            this.FullyBlocked = fullyBlocked;
            this.BlockedReasons = blockedReasons ?? throw new ArgumentNullException("blockedReasons");

            this.RawInputDataSize = rawInputDataSize ?? throw new ArgumentNullException("rawInputDataSize");
            this.RawInputPositions = rawInputPositions;

            this.ProcessedInputDataSize = processedInputDataSize ?? throw new ArgumentNullException("processedInputDataSize");
            this.ProcessedInputPositions = processedInputPositions;

            this.OutputDataSize = outputDataSize ?? throw new ArgumentNullException("outputDataSize");
            this.OutputPositions = outputPositions;

            this.PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException("physicalWrittenDataSize");

            this.StageGcStatistics = stageGcStatistics ?? throw new ArgumentNullException("stageGcStatistics");

            this.OperatorSummaries = operatorSummaries ?? throw new ArgumentNullException("operatorSummaries");

        }

        #endregion
    }
}