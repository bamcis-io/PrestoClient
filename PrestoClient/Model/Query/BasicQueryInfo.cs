using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Query
{
    /// <summary>
    /// Lightweight version of QueryInfo. Parts of the web UI depend on the fields
    /// being named consistently across these classes.
    /// From com.facebook.presto.server.BasicQueryInfo.java
    /// </summary>
    public class BasicQueryInfo
    {
        public string QueryId { get; set; }

        public SessionRepresentation Session { get; set; }

        public QueryState State { get; set; }

        public string MemoryPool { get; set; }

        public bool Scheduled { get; set; }

        public Uri Self { get; set; }

        public string Query { get; set; }

        public BasicQueryStats QueryStats { get; set; }

        /// <summary>
        /// TODO: This should be an enum
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorCode ErrorCode { get; set; }
    }
}