using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.StatementStats.java
    /// </summary>
    public class StatementStats
    {
        #region Public Properties

        public StatementState State { get; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool WaitingForPrerequisites { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Queued { get; }

        public bool Scheduled { get; }

        public int Nodes { get; }

        public int TotalSplits { get; }

        public int QueuedSplits { get; }

        public int RunningSplits { get; }

        public int CompletedSplits { get; }

        public Int64 UserTimeMillis { get; }

        public Int64 CpuTimeMillis { get; }

        public Int64 WallTimeMillis { get; }

        public Int64 QueuedTimeMillis { get; }
        public Int64 WaitingForPrerequisitesTimeMillis { get; }

        public Int64 ElapsedTimeMillis { get; }

        public Int64 ProcessedRows { get; }

        public Int64 ProcessedBytes { get; }

        public Int64 PeakMemoryBytes { get; }

        public StageStats RootStage { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public double ProgressPercentage
        {
            get
            {
                if (!this.Scheduled || this.TotalSplits == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Min(100, (this.CompletedSplits * 100.0) / this.TotalSplits);
                }
            }
        }

        #endregion

        #region Constructor

        [JsonConstructor]
        public StatementStats(
            string state,
            bool waitingForPrerequisites,
            bool queued,
            bool scheduled,
            int nodes,
            int totalSplits,
            int queuedSplits,
            int runningSplits,
            int completedSplits,
            long userTimeMillis,
            long cpuTimeMillis,
            long wallTimeMillis,
            long waitingForPrerequisitesTimeMillis,
            long queuedTimeMillis,
            long elapsedTimeMillis,
            long processedRows,
            long processedBytes,
            long peakMemoryBytes,
            StageStats rootStage
        )
        {
            this.State = (StatementState)Enum.Parse(typeof(StatementState), state);
            this.WaitingForPrerequisites = waitingForPrerequisites;
            this.Queued = queued;
            this.Scheduled = scheduled;
            this.Nodes = nodes;
            this.TotalSplits = totalSplits;
            this.QueuedSplits = queuedSplits;
            this.RunningSplits = runningSplits;
            this.CompletedSplits = completedSplits;
            this.UserTimeMillis = userTimeMillis;
            this.CpuTimeMillis = cpuTimeMillis;
            this.WallTimeMillis = wallTimeMillis;
            this.WaitingForPrerequisitesTimeMillis = waitingForPrerequisitesTimeMillis;
            this.QueuedTimeMillis = queuedTimeMillis;
            this.ElapsedTimeMillis = elapsedTimeMillis;
            this.ProcessedRows = processedRows;
            this.ProcessedBytes = processedBytes;
            this.PeakMemoryBytes = peakMemoryBytes;
            this.RootStage = rootStage;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("state", this.State)
                .Add("waitingForPrerequisites", this.WaitingForPrerequisites)
                .Add("queued", this.Queued)
                .Add("scheduled", this.Scheduled)
                .Add("nodes", this.Nodes)
                .Add("totalSplits", this.TotalSplits)
                .Add("queuedSplits", this.QueuedSplits)
                .Add("runningSplits", this.RunningSplits)
                .Add("completedSplits", this.CompletedSplits)
                .Add("userTimeMillis", this.UserTimeMillis)
                .Add("cpuTimeMillis", this.CpuTimeMillis)
                .Add("waitingForPrerequisitesTimeMillis", this.WaitingForPrerequisitesTimeMillis)
                .Add("wallTimeMillis", this.WallTimeMillis)
                .Add("queuedTimeMillis", this.QueuedTimeMillis)
                .Add("elapsedTimeMillis", this.ElapsedTimeMillis)
                .Add("processedRows", this.ProcessedRows)
                .Add("processedBytes", this.ProcessedBytes)
                .Add("peakMemoryBytes", this.PeakMemoryBytes)
                .Add("rootStage", this.RootStage)
                .ToString();
        }

        #endregion
    }
}