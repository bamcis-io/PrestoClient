using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// The actions section of the response from a v2 statement execution
    /// </summary>
    public class Actions
    {
        public string SetTransactionId { get; set; }

        public bool ClearTransactionId { get; set; }

        public IDictionary<string, string> SetSessionProperties { get; set; }

        public IEnumerable<string> ClearSessionProperties { get; set; }

        public IDictionary<string, string> AddPreparedStatements { get; set; }

        public IEnumerable<string> DeallocatePreparedStatements { get; set; }
    }
}