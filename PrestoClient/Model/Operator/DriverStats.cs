using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.DriverStats.java
    /// </summary>
    public class DriverStats
    {
        #region Public Properties

        public DateTime CreateTime { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan QueuedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

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

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan RawInputReadTime { get; }

        public DataSize ProcessedInputDataSize { get; }

        public long ProcessedInputPositions { get; }

        public DataSize OutputDataSize { get; }

        public long OutputPositions { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public IEnumerable<OperatorStats> OperatorStats { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DriverStats(
            DateTime createTime,
            DateTime startTime,
            DateTime endTime,
            TimeSpan queuedTime,
            TimeSpan elapsedTime,

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
            TimeSpan rawInputReadTime,

            DataSize processedInputDataSize,
            long processedInputPositions,

            DataSize outputDataSize,
            long outputPositions,

            DataSize physicalWrittenDataSize,

            IEnumerable<OperatorStats> operatorStats
            )
        {
            ParameterCheck.OutOfRange(rawInputPositions >= 0, "rawInputPositions");
            ParameterCheck.OutOfRange(processedInputPositions >= 0, "processedInputPositions");
            ParameterCheck.OutOfRange(outputPositions >= 0, "outputPositions");

            this.CreateTime = createTime;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.QueuedTime = queuedTime;
            this.ElapsedTime = elapsedTime;

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
            this.RawInputReadTime = rawInputReadTime;

            this.ProcessedInputDataSize = processedInputDataSize ?? throw new ArgumentNullException("processedInputDataSize");
            this.ProcessedInputPositions = processedInputPositions;

            this.OutputDataSize = outputDataSize ?? throw new ArgumentNullException("outputDataSize");
            this.OutputPositions = outputPositions;

            this.PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException("physicalWrittenDataSize");

            this.OperatorStats = operatorStats ?? throw new ArgumentNullException("operatorStats");
        }

        #endregion
    }
}
