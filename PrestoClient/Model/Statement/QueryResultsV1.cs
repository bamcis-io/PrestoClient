using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BAMCIS.PrestoClient.Serialization;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QueryResultsV1 : QueryResults
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Uri PartialCancelUri { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<List<dynamic>> Data { get; set; }

        public StatementStats Stats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Int64 UpdateCount { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the data rows to a CSV formatted array, each line
        /// in the array is CSV data and can be combined with a String.Join("\n", csvArray)
        /// function to get a single CSV string
        /// </summary>
        /// <returns>The data formatted as CSV with 1 line per index in the IEnumerable</returns>
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

        /// <summary>
        /// Converts the data rows into a JSON string using a Dictionary type mapping
        /// </summary>
        /// <returns>The JSON formatted string</returns>
        public string DataToJson()
        {
            if (Data != null)
            {
                Dictionary<string, Dictionary<string, object>[]> Wrapper = new Dictionary<string, Dictionary<string, object>[]>();
                Wrapper.Add("data", new Dictionary<string, object>[Data.Count()]);

                Column[] Columns = this.Columns.ToArray();
                int RowCounter = 0;

                foreach (List<dynamic> Row in Data)
                {
                    // Keep track of the column number
                    int Counter = 0;

                    Wrapper["data"][RowCounter] = new Dictionary<string, object>();

                    foreach (dynamic Column in Row)
                    {
                        Column Col = Columns[Counter++];

                        object Value = PrestoTypeMapping.Convert(Column.ToString(), Col.TypeSignature.RawType);

                        Wrapper["data"][RowCounter].Add(Col.Name, Value);
                    }

                    RowCounter++;
                }

                return JsonConvert.SerializeObject(Wrapper);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}