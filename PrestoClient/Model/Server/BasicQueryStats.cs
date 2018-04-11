using BAMCIS.PrestoClient.Model.Execution;
using BAMCIS.PrestoClient.Model.Execution.Scheduler;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// Lightweight version of QueryStats. Parts of the web UI depend on the fields
    /// being named consistently across these classes.
    /// 
    /// From com.facebook.presto.server.BasicQueryStats.java
    /// </summary>
    public class BasicQueryStats
    {
        public DateTime CreateTime { get; }

        public DateTime EndTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ElapsedTime { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan ExecutionTime { get; }

        public int TotalDrivers { get; }

        public int QueuedDrivers { get; }

        public int RunningDrivers { get; }

        public int CompletedDrivers { get; }

        public double CumulativeUserMemory { get; }

        public DataSize UserMemoryReservation { get; }

        public DataSize PeakMemoryReservation { get; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalCpuTime { get; }

        public bool FullyBlocked { get; }

        public HashSet<BlockedReason> BlockedReasons { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage { get; }

        #region Constructors

        [JsonConstructor]
        public BasicQueryStats(
            DateTime createTime,
            DateTime endTime,
            TimeSpan elapsedTime,
            TimeSpan executionTime,
            int totalDrivers,
            int queuedDrivers,
            int runningDrivers,
            int completedDrivers,
            double cumulativeUserMemory,
            DataSize userMemoryReservation,
            DataSize peakUserMemoryReservation,
            TimeSpan totalCpuTime,
            bool fullyBlocked,
            HashSet<BlockedReason> blockedReasons,
            double progressPercentage
            )
        {
            ParameterCheck.OutOfRange(totalDrivers >= 0, "totalDrivers", "Total drivers cannot be negative.");
            ParameterCheck.OutOfRange(queuedDrivers >= 0, "queuedDrivers", "Queued drivers cannot be negative.");
            ParameterCheck.OutOfRange(runningDrivers >= 0, "runningDrivers", "Running drivers cannot be negative.");
            ParameterCheck.OutOfRange(completedDrivers >= 0, "completedDrivers", "Completed drivers cannot be negative.");

            this.CreateTime = createTime;
            this.EndTime = endTime;

            this.ElapsedTime = elapsedTime;

            this.TotalDrivers = totalDrivers;
            this.QueuedDrivers = queuedDrivers;
            this.RunningDrivers = runningDrivers;
            this.CompletedDrivers = completedDrivers;

            this.CumulativeUserMemory = cumulativeUserMemory;
            this.UserMemoryReservation = userMemoryReservation;
            this.PeakMemoryReservation = peakUserMemoryReservation;
            this.TotalCpuTime = totalCpuTime;

            this.FullyBlocked = fullyBlocked;
            this.BlockedReasons = blockedReasons ?? throw new ArgumentNullException("blockedReasons");
            this.ProgressPercentage = progressPercentage;
        }

        public BasicQueryStats(QueryStats queryStats) : this( 
            queryStats.CreateTime,
            queryStats.EndTime,
            queryStats.ElapsedTime,
            queryStats.ExecutionTime,
            queryStats.TotalDrivers,
            queryStats.QueuedDrivers,
            queryStats.RunningDrivers,
            queryStats.CompletedDrivers,
            queryStats.CumulativeUserMemory,
            queryStats.UserMemoryReservation,
            queryStats.PeakUserMemoryReservation,
            queryStats.TotalCpuTime,
            queryStats.FullyBlocked,
            queryStats.BlockedReasons,
            queryStats.ProgressPercentage
            )
        { }

        #endregion
    }
}