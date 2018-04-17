using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// A response from executing a query
    /// </summary>
    public class ExecuteQueryV1Response
    {
        #region Private Properties

        private List<QueryResultsV1> _Responses = new List<QueryResultsV1>();

        #endregion

        #region Public Properties

        /// <summary>
        /// The incremental response status info objects
        /// </summary>
        public IReadOnlyList<IQueryStatusInfo> Responses
        {
            get
            {
                return this._Responses;
            }
        }

        /// <summary>
        /// The data from the responses
        /// </summary>
        public IEnumerable<List<dynamic>> Data
        {
            get
            {
                return this._Responses.Where(x => x != null && x.Data != null).SelectMany(x => x.GetData()).Where(x => x != null);
            }
        }

        /// <summary>
        /// The columns requested in the query
        /// </summary>
        public IReadOnlyList<Column> Columns { get; }

        /// <summary>
        /// Indicates whether the query was successfully closed by the client.
        /// </summary>
        public bool QueryClosed { get; }

        /// <summary>
        /// If deserialization fails, the will contain the thrown exception. Otherwise, 
        /// this property is null.
        /// </summary>
        public Exception LastError { get; }

        #endregion

        #region Constructors

        internal ExecuteQueryV1Response(IEnumerable<QueryResultsV1> results, bool closed)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }

            this._Responses = results.ToList();
            this.QueryClosed = closed;
            this.LastError = null;

            if (this._Responses.Any())
            {
                // The first response may not have any column data, so find the first
                // resposne that does and pull the columns from that
                this.Columns = this._Responses.First(x => x.Columns != null).Columns.ToList();
            }
            else
            {
                this.Columns = new Column[0];
            }
        }

        internal ExecuteQueryV1Response(IEnumerable<QueryResultsV1> results, bool closed, Exception lastError) : this(results, closed)
        {
            this.LastError = lastError;
        }

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
                Dictionary<string, Dictionary<string, object>[]> Wrapper = new Dictionary<string, Dictionary<string, object>[]>
                {
                    { "data", new Dictionary<string, object>[Data.Count()] }
                };

                int RowCounter = 0;

                foreach (List<dynamic> Row in Data)
                {
                    // Keep track of the column number
                    int Counter = 0;

                    Wrapper["data"][RowCounter] = new Dictionary<string, object>();

                    foreach (dynamic Column in Row)
                    {
                        Column Col = this.Columns[Counter++];

                        object Value = null;

                        if (Column != null)
                        {
                            Value = PrestoTypeMapping.Convert(Column.ToString(), Col.TypeSignature.RawType);
                        }

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