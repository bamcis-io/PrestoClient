using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class StatementStats
    {
        public StatementState State { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Queued { get; set; }

        public bool Scheduled { get; set; }

        public int Nodes { get; set; }

        public int TotalSplits { get; set; }

        public int QueuedSplits { get; set; }

        public int RunningSplits { get; set; }

        public int CompletedSplits { get; set; }

        public Int64 UserTimeMillis { get; set; }

        public Int64 CpuTimeMillis { get; set; }

        public Int64 WallTimeMillis { get; set; }

        public Int64 QueuedTimeMillis { get; set; }

        public Int64 ElapsedTimeMillis { get; set; }

        public Int64 ProcessedRows { get; set; }

        public Int64 ProcessedBytes { get; set; }

        public Int64 PeakMemoryBytes { get; set; }

        public StageStats RootStage { get; set; }
    }
}