namespace BAMCIS.PrestoClient
{
    /// <summary>
    /// Available Presto headers
    /// https://github.com/prestodb/presto/blob/master/presto-client/src/main/java/com/facebook/presto/client/PrestoHeaders.java
    /// </summary>
    public class PrestoHeader
    {
        private PrestoHeader(string value) { Value = value; }

        public string Value { get; set; }

        public static readonly PrestoHeader PRESTO_USER = new PrestoHeader("X-Presto-User");
        public static readonly PrestoHeader PRESTO_SOURCE = new PrestoHeader("X-Presto-Source");
        public static readonly PrestoHeader PRESTO_CATALOG = new PrestoHeader("X-Presto-Catalog");
        public static readonly PrestoHeader PRESTO_SCHEMA = new PrestoHeader("X-Presto-Schema");
        public static readonly PrestoHeader PRESTO_TIME_ZONE = new PrestoHeader("X-Presto-Time-Zone");
        public static readonly PrestoHeader PRESTO_LANGUAGE = new PrestoHeader("X-Presto-Language");
        public static readonly PrestoHeader PRESTO_SESSION = new PrestoHeader("X-Presto-Session");
        public static readonly PrestoHeader PRESTO_SET_CATALOG = new PrestoHeader("X-Presto-Set-Catalog");
        public static readonly PrestoHeader PRESTO_SET_SCHEMA = new PrestoHeader("X-Presto-Set-Schema");
        public static readonly PrestoHeader PRESTO_SET_SESSION = new PrestoHeader("X-Presto-Set-Session");
        public static readonly PrestoHeader PRESTO_CLEAR_SESSION = new PrestoHeader("X-Presto-Clear-Session");
        public static readonly PrestoHeader PRESTO_PREPARED_STATEMENT = new PrestoHeader("X-Presto-Prepared-Statement");
        public static readonly PrestoHeader PRESTO_ADDED_PREPARE = new PrestoHeader("X-Presto-Added-Prepare");
        public static readonly PrestoHeader PRESTO_DEALLOCATED_PREPARE = new PrestoHeader("X-Presto-Deallocated-Prepare");
        public static readonly PrestoHeader PRESTO_TRANSACTION_ID = new PrestoHeader("X-Presto-Transaction-Id");
        public static readonly PrestoHeader PRESTO_STARTED_TRANSACTION_ID = new PrestoHeader("X-Presto-Started-Transaction-Id");
        public static readonly PrestoHeader PRESTO_CLEAR_TRANSACTION_ID = new PrestoHeader("X-Presto-Clear-Transaction-Id");
        public static readonly PrestoHeader PRESTO_CLIENT_INFO = new PrestoHeader("X-Presto-Client-Info");
        public static readonly PrestoHeader PRESTO_CLIENT_TAGS = new PrestoHeader("X-Presto-Client-Tags");

        public static readonly PrestoHeader PRESTO_CURRENT_STATE = new PrestoHeader("X-Presto-Current-State");
        public static readonly PrestoHeader PRESTO_MAX_WAIT = new PrestoHeader("X-Presto-Max-Wait");
        public static readonly PrestoHeader PRESTO_MAX_SIZE = new PrestoHeader("X-Presto-Max-Size");
        public static readonly PrestoHeader PRESTO_TASK_INSTANCE_ID = new PrestoHeader("X-Presto-Task-Instance-Id");
        public static readonly PrestoHeader PRESTO_PAGE_TOKEN = new PrestoHeader("X-Presto-Page-Sequence-Id");
        public static readonly PrestoHeader PRESTO_PAGE_NEXT_TOKEN = new PrestoHeader("X-Presto-Page-End-Sequence-Id");
        public static readonly PrestoHeader PRESTO_BUFFER_COMPLETE = new PrestoHeader("X-Presto-Buffer-Complete");

        public static readonly PrestoHeader PRESTO_DATA_NEXT_URI = new PrestoHeader("X-Presto-Data-Next-Uri");

        public override string ToString()
        {
            return this.Value;
        }
    }
}