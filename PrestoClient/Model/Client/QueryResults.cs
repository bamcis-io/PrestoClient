using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// The basis of return data from presto when either submitting a query statement
    /// or following the nextUri return data
    /// 
    /// From com.facebook.presto.client.QueryResults.java, but implemented here as a base
    /// class to be shared between QueryResultsV'X' for later versions.
    /// </summary>
    public abstract class QueryResults
    {
        #region Public Properties

        /// <summary>
        /// The unique Id for this query
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// A uri to retrieve detailed information about the query
        /// </summary>
        public Uri InfoUri { get; }

        /// <summary>
        /// This property will be null when the query has successfully completed
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri NextUri { get; }

        /// <summary>
        /// The columns included in the query results
        /// </summary>
        public IEnumerable<Column> Columns { get; }

        /// <summary>
        /// This property will be null unless there is an error with the query
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryError Error { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Abstract constructor for inherited classes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="infoUri"></param>
        /// <param name="nextUri"></param>
        /// <param name="columns"></param>
        /// <param name="error"></param>
        public QueryResults(
            string id,
            Uri infoUri,
            Uri nextUri,
            IEnumerable<Column> columns,
            QueryError error
            )
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            this.Id = id;
            this.InfoUri = infoUri ?? throw new ArgumentNullException("infoUri");
            this.NextUri = nextUri;
            this.Columns = columns;
            this.Error = error;
        }

        #endregion

        #region Public Methods


        #endregion
    }
}