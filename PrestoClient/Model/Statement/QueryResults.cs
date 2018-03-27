using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// The basis of return data from presto when either submitting a query statement
    /// or following the nextUri return data
    /// </summary>
    public abstract class QueryResults
    {
        /// <summary>
        /// The unique Id for this query
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A uri to retrieve detailed information about the query
        /// </summary>
        public Uri InfoUri { get; set; }

        /// <summary>
        /// This property will be null when the query has successfully completed
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri NextUri { get; set; }

        /// <summary>
        /// The columns included in the query results
        /// </summary>
        public IEnumerable<Column> Columns { get; set; }

        /// <summary>
        /// This property will be null unless there is an error with the query
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryError Error { get; set; }
    }
}