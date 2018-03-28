using System;
using System.Collections.Generic;
using System.Globalization;

namespace BAMCIS.PrestoClient.Model
{
    public class QueryOptions
    {
        public TimeZoneInfo TimeZone { get; set; }

        public CultureInfo Language { get; set; }

        public string Session { get; set; }

        public string SetSchema { get; set; }

        public string SetSession { get; set; }

        public string ClearSession { get; set; }

        public string PreparedStatement { get; set; }

        public string AddedPrepare { get; set; }

        public string DeallocatedPrepare { get; set; }

        public string TransactionId { get; set; }

        public string StartedTransactionId { get; set; }

        public string ClearTransactionId { get; set; }

        public string ClientInfo { get; set; }

        public string CurrentState { get; set; }

        public int MaxWait { get; set; }

        public int MaxSize { get; set; }

        public string TaskInstanceId { get; set; }

        public string SequenceId { get; set; }

        public string EndSequenceId { get; set; }

        public string BufferComplete { get; set; }

        public IEnumerable<string> ClientTags { get; set; }
    }
}