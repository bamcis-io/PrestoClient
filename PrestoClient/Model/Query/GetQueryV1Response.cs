using BAMCIS.PrestoClient.Model.Execution;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Query
{
    /// <summary>
    /// A response object for a request of the details of a specific query.
    /// </summary>
    public class GetQueryV1Response
    {
        #region Public Properties

        /// <summary>
        /// The raw JSON content returned from presto
        /// </summary>
        public string RawContent { get; }

        /// <summary>
        /// The deserialized json. If deserialization fails, this will be null.
        /// </summary>
        public QueryInfo QueryInfo { get; }

        /// <summary>
        /// Indicates whether deserialization was successful.
        /// </summary>
        public bool DeserializationSucceeded { get; }

        /// <summary>
        /// If deserialization fails, the will contain the thrown exception. Otherwise, 
        /// this property is null.
        /// </summary>
        public Exception LastError { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new response from the JSON object string returned from presto.
        /// </summary>
        /// <param name="rawContent">The JSON object of query details</param>
        internal GetQueryV1Response(string rawContent)
        {
            this.RawContent = rawContent;

            if (!String.IsNullOrEmpty(this.RawContent))
            {
                try
                {
                    this.QueryInfo = JsonConvert.DeserializeObject<QueryInfo>(this.RawContent);
                    this.DeserializationSucceeded = true;
                    this.LastError = null;
                }
                catch (Exception e)
                {
                    this.DeserializationSucceeded = false;
                    this.LastError = e;
                    this.QueryInfo = null;
                }
            }
        }

        #endregion
    }
}