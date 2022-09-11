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

        public int PipelineId { get; }

        public DateTime FirstStartTime { get; }

        public DateTime LastStartTime { get; }

        public DateTime LastEndTime { get; }

        public bool InputPipeline { get; }

        public bool OutputPipeline { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int QueuedPartitionedDrivers { get; }

        public int RunningDrivers { get; }

        public int RunningPartitionedDrivers { get; }

        public int BlockedDrivers { get; }

        public int CompletedDrivers { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize RevocableMemoryReservation { get; }

        public DistributionSnapshot QueuedTime { get; }

        public DistributionSnapshot ElapsedTime { get; }

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

        public IEnumerable<OperatorStats> OperatorSummaries { get; }

        public IEnumerable<DriverStats> Drivers { get; }

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
