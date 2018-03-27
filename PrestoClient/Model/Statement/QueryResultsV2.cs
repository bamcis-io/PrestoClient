using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QueryResultsV2 : QueryResults
    {
        #region Public Properties

        public bool NextUriDone { get; set; }

        public Uri FinalUri { get; set; }

        public IEnumerable<Uri> DataUris { get; set; }

        public QuerySubmissionResultsActions Actions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<List<dynamic>> Data { get; set; }

        #endregion

        #region Public Methods

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