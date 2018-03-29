using BAMCIS.PrestoClient.Model.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QueryResultsV2 : QueryResults, IQueryData
    {
        #region Public Properties

        public bool NextUriDone { get; }

        public Uri FinalUri { get; }

        public IEnumerable<Uri> DataUris { get; }

        public Actions Actions { get; }

        public IEnumerable<List<dynamic>> Data { get; set; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryResultsV2(
            string id,
            Uri infoUri,
            Uri finalUri,
            Uri nextUri,
            bool nextUriDone,
            IEnumerable<Column> columns,
            IEnumerable<Uri> dataUris,
            Actions actions,
            QueryError error
            ) : base(id, infoUri, nextUri, columns, error)
        {
            this.FinalUri = finalUri;
            this.NextUriDone = nextUriDone;
            this.DataUris = dataUris;
            this.Actions = actions;
        }

        #endregion

        #region Public Methods

        public IEnumerable<List<dynamic>> GetData()
        {
            return this.Data;
        }

        public IEnumerable<string> DataToCsv()
        {
            if (this.Data != null)
            {
                foreach (var Item in this.Data)
                {
                    StringBuilder SB = new StringBuilder();

                    foreach (var Column in Item)
                    {
                        SB.Append($"\"{Column}\",");
                    }

                    // Remove last comma
                    SB.Length = SB.Length - 1;

                    yield return SB.ToString();
                }
            }
            else
            {
                throw new ArgumentNullException("Data", "The data in this query result is null.");
            }
        }

        #endregion
    }
}