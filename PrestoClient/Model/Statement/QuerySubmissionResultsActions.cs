using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QuerySubmissionResultsActions
    {
        public string SetTransactionId { get; set; }

        public bool ClearTransactionId { get; set; }

        public IDictionary<string, string> SetSessionProperties { get; set; }

        public IEnumerable<string> ClearSessionProperties { get; set; }

        public IDictionary<string, string> AddPreparedStatements { get; set; }

        public IEnumerable<string> DeallocatePreparedStatements { get; set; }
    }
}