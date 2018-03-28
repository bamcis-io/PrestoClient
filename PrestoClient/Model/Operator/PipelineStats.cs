using BAMCIS.PrestoClient.Model.Execution;
using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.PipelineStats.java
    /// </summary>
    public class PipelineStats
    {
        #region Public Properties

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

        public DataSize UserMemoryReservation { get; set; }

        public DataSize RevocableMemoryReservation { get; set; }

        public DataSize SystemMemoryReservation { get; set; }

        public DistributionSnapshot QueuedTime { get; set; }

        public DistributionSnapshot ElapsedTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalScheduledTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalUserTime { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalBlockedTime { get; set; }

        public bool FullyBlocked { get; set; }

        public HashSet<BlockedReason> BlockedReasons { get; set; }

        public DataSize RawInputDataSize { get; set; }

        public long RawInputPositions { get; set; }

        public DataSize ProcessedInputDataSize { get; set; }

        public long ProcessedInputPositions { get; set; }

        public DataSize OutputDataSize { get; set; }

        public long OutputPositions { get; set; }

        public DataSize PhysicalWrittenDataSize { get; set; }

        public IEnumerable<OperatorStats> OperatorSummaries { get; set; }

        public IEnumerable<DriverStats> Drivers { get; set; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PipelineStats(
            int pipelineId,

            DateTime firstStartTime,
            DateTime lastStartTime,
            DateTime lastEndTime,

            bool inputPipeline,
            bool outputPipeline,

            int totalDrivers,
            int queuedDrivers,
            int queuedPartitionedDrivers,
            int runningDrivers,
            int runningPartitionedDrivers,
            int blockedDrivers,
            int completedDrivers,

            DataSize userMemoryReservation,
            DataSize revocableMemoryReservation,
            DataSize systemMemoryReservation,

            DistributionSnapshot queuedTime,
            DistributionSnapshot elapsedTime,

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

            IEnumerable<OperatorStats> operatorSummaries,
            IEnumerable<DriverStats> drivers

            )
        {
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers");
            ParameterCheck.OutOfRange(queuedPartitionedDrivers >= 0, "queuedPartitionedDrivers");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers");
            ParameterCheck.OutOfRange(runningPartitionedDrivers >= 0, "runningPartitionedDrivers");
            ParameterCheck.OutOfRange(blockedDrivers >= 0, "blockedDrivers");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers");
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions");

            this.PipelineId = pipelineId;

            this.FirstStartTime = firstStartTime;
            this.LastStartTime = lastStartTime;
            this.LastEndTime = lastEndTime;

            this.InputPipeline = inputPipeline;
            this.OutputPipeline = outputPipeline;

            this.TotalDrivers = totalDrivers;
            this.QueuedDrivers = queuedDrivers;
            this.QueuedPartitionedDrivers = queuedPartitionedDrivers;
            this.RunningDrivers = runningDrivers;
            this.RunningPartitionedDrivers = runningPartitionedDrivers;
            this.BlockedDrivers = blockedDrivers;
            this.CompletedDrivers = completedDrivers;

            this.UserMemoryReservation = userMemoryReservation ?? throw new ArgumentNullException("userMemoryReservation");
            this.RevocableMemoryReservation = revocableMemoryReservation ?? throw new ArgumentNullException("revocableMemoryReservation");
            this.SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException("systemMemoryReservation");

            this.QueuedTime = queuedTime;
            this.ElapsedTime = elapsedTime;
            this.TotalScheduledTime = totalScheduledTime;

            this.TotalCpuTime = totalCpuTime;
            this.TotalUserTime = totalUserTime;
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

            this.OperatorSummaries = operatorSummaries ?? throw new ArgumentNullException("operatorSummaries");
            this.Drivers = drivers ?? throw new ArgumentNullException("drivers");
        }

        #endregion
    }
}
