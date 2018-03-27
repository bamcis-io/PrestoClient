using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class StageId
    {
        #region Public Properties

        public QueryId QueryId { get; }

        public int Id { get; }

        #endregion

        #region Constructors

        public StageId(string combinedId)
        {
            string[] Parts = combinedId.Split('.');

            if (Parts.Length < 2)
            {
                throw new ArgumentException("The combined id must be a query id and id separated with a '.' character.");
            }

            this.QueryId = new QueryId(Parts[0]);
            this.Id = Int32.Parse(Parts[1]);
        }

        public StageId(string queryId, int id)
        {
            this.QueryId = new QueryId(queryId);
            this.Id = id;
        }

        public StageId(QueryId queryId, int id)
        {
            this.QueryId = queryId ?? throw new ArgumentNullException("queryId", "Query id cannot be null.");
            this.Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.QueryId.ToString()}.{this.Id}";
        }

        #endregion
    }
}
