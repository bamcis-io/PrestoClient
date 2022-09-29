using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.QueryInfo.java
    /// </summary>
    public class QueryInfo
    {
        #region Public Properties

        public QueryId QueryId { get; }

        public SessionRepresentation Session { get; }

        public QueryState State { get; }

        public bool Scheduled { get; }

        public Uri Self { get; }

        public IEnumerable<string> FieldNames { get; }

        public string Query { get; }

        public QueryStats QueryStats { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetCatalog { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetSchema { get; }

        public IDictionary<string, string> SetSessionProperties { get; }

        public HashSet<string> ResetSessionProperties { get; }

        public IDictionary<string, string> AddedPreparedStatements { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<string> DeallocatedPreparedStatements { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TransactionId StartedTransactionId { get; }

        public bool ClearTransactionId { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string UpdateType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public StageInfo OutputStage { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FailureInfo FailureInfo { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorType ErrorType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorCode ErrorCode { get; }

        public bool FinalQueryInfo { get; }

        public HashSet<Input> Inputs { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Output Output { get; }

        public bool CompleteInfo { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string ResourceGroupName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryInfo(
            QueryId queryId,
            SessionRepresentation session,
            QueryState state,
            bool scheduled,
            Uri self,
            IEnumerable<string> fieldNames,
            string query,
            QueryStats queryStats,
            string setCatalog,
            string setSchema,
            IDictionary<string, string> setSessionProperties,
            HashSet<string> resetSessionProperties,
            IDictionary<string, string> addedPreparedStatements,
            HashSet<string> deallocatedPreparedStatemetns,
            TransactionId startedTransactionId,
            bool clearTransactionId,
            string updateType,
            StageInfo outputStage,
            FailureInfo failureInfo,
            ErrorCode errorCode,
            HashSet<Input> inputs,
            Output output,
            bool completeInfo,
            string resourceGroupName
            )
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query");
            }

            this.QueryId = queryId ?? throw new ArgumentNullException("queryId");
            this.Session = session ?? throw new ArgumentNullException("session");
            this.State = state;
            this.Scheduled = scheduled;
            this.Self = self ?? throw new ArgumentNullException("self");
            this.FieldNames = fieldNames ?? throw new ArgumentNullException("fieldNames");
            this.Query = query;
            this.QueryStats = queryStats ?? throw new ArgumentNullException("queryStats");
            this.SetCatalog = setCatalog;
            this.SetSchema = setSchema;
            this.SetSessionProperties = setSessionProperties ?? throw new ArgumentNullException("setSessionProperties");
            this.ResetSessionProperties = resetSessionProperties ?? throw new ArgumentNullException("resetSessionProperties");
            this.AddedPreparedStatements = addedPreparedStatements ?? throw new ArgumentNullException("addedPreparedStatements");
            this.DeallocatedPreparedStatements = deallocatedPreparedStatemetns; // ?? throw new ArgumentNullException("deallocatedPreparedStatements");
            this.StartedTransactionId = startedTransactionId;
            this.ClearTransactionId = clearTransactionId;
            this.UpdateType = updateType;
            this.OutputStage = outputStage;
            this.FailureInfo = failureInfo;
            this.ErrorType = errorCode == null ? ErrorType.NONE : errorCode.Type;
            this.ErrorCode = errorCode;
            this.Inputs = inputs ?? throw new ArgumentNullException("inputs");
            this.Output = output;
            this.CompleteInfo = completeInfo;
            this.ResourceGroupName = resourceGroupName;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("queryId", this.QueryId)
                .Add("state", this.State)
                .Add("fieldNames", this.FieldNames)
                .ToString();
        }

        #endregion
    }
}
