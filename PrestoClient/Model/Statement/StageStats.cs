using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class StageStats
    {
        public string StageId { get; set; }

        public string State { get; set; }

        public bool Done { get; set; }

        public int Nodes { get; set; }

        public int TotalSplits { get; set; }

        public int QueuedSplits { get; set; }

        public int RunningSplits { get; set; }

        public int CompletedSplits { get; set; }

        public Int64 UserTimeMillis { get; set; }

        public Int64 CpuTimeMillis { get; set; }

        public Int64 WallTimeMillis { get; set; }

        public Int64 ProcessedRows { get; set; }

        public Int64 ProcessedBytes { get; set; }

        public IEnumerable<StageStats> SubStages { get; set; }
    }
}