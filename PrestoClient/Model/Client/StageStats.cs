using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.StageStats.java
    /// </summary>
    public class StageStats
    {
        #region Public Properties

        public string StageId { get; }

        public string State { get; }

        public bool Done { get; }

        public int Nodes { get; }

        public int TotalSplits { get; }

        public int QueuedSplits { get; }

        public int RunningSplits { get; }

        public int CompletedSplits { get; }

        public Int64 UserTimeMillis { get; }

        public Int64 CpuTimeMillis { get; }

        public Int64 WallTimeMillis { get; }

        public Int64 ProcessedRows { get; }

        public Int64 ProcessedBytes { get; }

        public IEnumerable<StageStats> SubStages { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StageStats(
            string stageId,
            string state,
            bool done,
            int nodes,
            int totalSplits,
            int queuedSplits,
            int runningSplits,
            int completedSplits,
            long userTimeMillis,
            long cpuTimeMillis,
            long wallTimeMillis,
            long processedRows,
            long processedBytes,
            IEnumerable<StageStats> subStages
            )
        {
            if (String.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException("state");
            }

            this.StageId = stageId;
            this.State = state;
            this.Done = done;
            this.Nodes = nodes;
            this.TotalSplits = totalSplits;
            this.QueuedSplits = queuedSplits;
            this.RunningSplits = runningSplits;
            this.CompletedSplits = completedSplits;
            this.UserTimeMillis = userTimeMillis;
            this.CpuTimeMillis = cpuTimeMillis;
            this.WallTimeMillis = wallTimeMillis;
            this.ProcessedRows = processedRows;
            this.ProcessedBytes = processedBytes;
            this.SubStages = subStages ?? throw new ArgumentNullException("subStages");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("state", this.State)
                .Add("done", this.Done)
                .Add("nodes", this.Nodes)
                .Add("totalSplits", this.TotalSplits)
                .Add("queuedSplits", this.QueuedSplits)
                .Add("runningSplits", this.RunningSplits)
                .Add("completedSplits", this.CompletedSplits)
                .Add("userTimeMillis", this.UserTimeMillis)
                .Add("cpuTimeMillis", this.CpuTimeMillis)
                .Add("wallTimeMillis", this.WallTimeMillis)
                .Add("processedRows", this.ProcessedRows)
                .Add("processedBytes", this.ProcessedBytes)
                .Add("subStages", this.SubStages)
                .ToString();
        }

        #endregion
    }
}