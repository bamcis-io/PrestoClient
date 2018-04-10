using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BAMCIS.PrestoClient.Serialization;
using BAMCIS.PrestoClient.Model.Client;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QueryResultsV1 : QueryResults, IQueryData, IQueryStatusInfo
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

        #region Constructors

        /// <summary>
        /// Default constructor for query results
        /// </summary>
        /// <param name="id"></param>
        /// <param name="infoUri"></param>
        /// <param name="partialCancelUri"></param>
        /// <param name="nextUri"></param>
        /// <param name="columns"></param>
        /// <param name="data"></param>
        /// <param name="stats"></param>
        /// <param name="error"></param>
        /// <param name="updateType"></param>
        /// <param name="updateCount"></param>
        [JsonConstructor]
        public QueryResultsV1(
            string id,
            Uri infoUri,
            Uri partialCancelUri,
            Uri nextUri,
            IEnumerable<Column> columns,
            IEnumerable<List<dynamic>> data,
            StatementStats stats,
            QueryError error,
            string updateType,
            long updateCount
            ) : base(id, infoUri, nextUri, columns, error)
        {
            if (data != null && columns == null)
            {
                throw new ArgumentException("Data present without columns");
            }

            this.PartialCancelUri = partialCancelUri;
            this.Data = data;
            this.Stats = stats ?? throw new ArgumentNullException("stats");
            this.UpdateType = updateType;
            this.UpdateCount = updateCount;
        }

        #endregion

        #region Public Methods

        #region IQueryData

        /// <summary>
        /// Returns the data from the query
        /// </summary>
        /// <returns>Any available data from this query result</returns>
        public IEnumerable<List<dynamic>> GetData()
        {
            return this.Data;
        }

        #endregion

        #region IQueryStatus

        public string GetId()
        {
            return this.Id;
        }

        public Uri GetInfoUri()
        {
            return this.InfoUri;
        }

        public Uri GetPartialCancelUri()
        {
            return this.PartialCancelUri;
        }

        public Uri GetNextUri()
        {
            return this.NextUri;
        }

        public IEnumerable<Column> GetColumns()
        {
            return this.Columns;
        }

        public StatementStats GetStats()
        {
            return this.Stats;
        }

        public QueryError GetError()
        {
            return this.Error;
        }

        public string GetUpdateType()
        {
            return this.UpdateType;
        }

        public long GetUpdateCount()
        {
            return this.UpdateCount;
        }

        #endregion

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
            if (this.Data != null && this.Columns != null)
            {
                Dictionary<string, Dictionary<string, object>[]> Wrapper = new Dictionary<string, Dictionary<string, object>[]>
                {
                    { "data", new Dictionary<string, object>[this.Data.Count()] }
                };

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


        public static bool TryParse(string content, out QueryResultsV1 result, out Exception ex)
        {
            ex = null;

            if (!String.IsNullOrEmpty(content))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<QueryResultsV1>(content);
                    return true;
                }
                catch (Exception e)
                {
                    result = null;
                    ex = e;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

       
        #endregion
    }
}