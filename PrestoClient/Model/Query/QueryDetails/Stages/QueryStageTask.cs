using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStageTask
    {
        public QueryStageTaskStatus TaskStatus { get; set; }

        public DateTime LastHeartBeat { get; set; }

        public QueryStageOutputBufferInfo OutputBuffers { get; set; }

        public IEnumerable<string> NoMoreSplits { get; set; }

        public QueryStageTaskStats Stats { get; set; }

        public bool NeedsPlan { get; set; }

        public bool Complete { get; set; }
    }
}
