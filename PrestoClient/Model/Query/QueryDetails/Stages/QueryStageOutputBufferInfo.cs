using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStageOutputBufferInfo
    {
        public string Type { get; set; }
        
        public QueryState State { get; set; }

        public bool CanAddBuffers { get; set; }

        public bool CanAddPages { get; set; }

        public int TotalBufferedBytes { get; set; }

        public int TotalBufferedPages { get; set; }

        public int TotalRowsSent { get; set; }

        public int TotalPagesSent { get; set; }

        /// <summary>
        /// TODO: Data example unknown
        /// </summary>
        public IEnumerable<object> Buffers { get; set; }
    }
}
