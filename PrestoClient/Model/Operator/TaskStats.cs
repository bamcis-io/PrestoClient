using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.TaskStats.java
    /// </summary>
    public class TaskStats
    {
        #region Public Properties

        public DateTime CreateTime { get; }

        public DateTime FirstStartTime { get; }

        public DateTime LastStartTime { get; }

        public DateTime LastEndTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan QueuedTime { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int QueuedPartitionedDrivers { get; }

        public int RunningDrivers { get; }

        public int RunningPartitionedDrivers { get; }

        public int BlockedDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeUserMemory { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize RevocableMemoryReservation { get; }

        public DataSize SystemMemoryReservation { get; }

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

        public IEnumerable<PipelineStats> Pipelines { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TaskStats(
            DateTime createTime,
            DateTime firstStartTime,
            DateTime lastStartTime,
            DateTime lastEndTime,
            DateTime endTime,
            TimeSpan elapsedTime,
            TimeSpan queuedTime,

            int totalDrivers,
            int queuedDrivers,
            int queuedPartitionedDrivers,
            int runningDrivers,
            int runningPartitionedDrivers,
            int blockedDrivers,
            int completedDrivers,

            double cumulativeUserMemory,
            DataSize userMemoryReservation,
            DataSize revocableMemoryReservation,
            DataSize systemMemoryReservation,

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

            IEnumerable<PipelineStats> pipelines            
        )
        {
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers");
            ParameterCheck.OutOfRange(queuedPartitionedDrivers >= 0, "queuedPartitionedDrivers");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers");
            ParameterCheck.OutOfRange(runningPartitionedDrivers >= 0, "runningPartitionedDrivers");
            ParameterCheck.OutOfRange(blockedDrivers >= 0, "blockedDrivers ");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers");
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions");

            this.CreateTime = createTime;
            this.FirstStartTime = firstStartTime;
            this.LastStartTime = lastStartTime;
            this.LastEndTime = lastEndTime;
            this.EndTime = endTime;
            this.ElapsedTime = elapsedTime;
            this.QueuedTime = queuedTime;

           
            this.TotalDrivers = totalDrivers;
            this.QueuedDrivers = queuedDrivers;
            this.QueuedPartitionedDrivers = queuedPartitionedDrivers;

            this.RunningDrivers = runningDrivers;
            this.RunningPartitionedDrivers = runningPartitionedDrivers;
            this.BlockedDrivers = blockedDrivers;
            this.CompletedDrivers = completedDrivers;

            this.CumulativeUserMemory = cumulativeUserMemory;
            this.UserMemoryReservation = userMemoryReservation ?? throw new ArgumentNullException("userMemoryReservation");
            this.RevocableMemoryReservation = revocableMemoryReservation ?? throw new ArgumentNullException("revocableMemoryReservation");
            this.SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException("systemMemoryReservation");

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

            this.PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException("outputDataSize");

            this.Pipelines = pipelines ?? throw new ArgumentNullException("pipelines");
        }

        #endregion
    }
}
