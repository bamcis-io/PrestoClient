using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.SPI.Memory;
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

        public string QueryId { get; set; }

        public SessionRepresentation Session { get; set; }

        public QueryState State { get; set; }

        public MemoryPoolId MemoryPool { get; set; }

        public bool Scheduled { get; set; }

        public Uri Self { get; set; }

        public IEnumerable<string> FieldNames { get; set; }

        public string Query { get; set; }

        public QueryStats QueryStats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetCatalog { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string SetSchema { get; set; }

        public IDictionary<string, string> SetSessionProperties { get; set; }

        public HashSet<string> ResetSessionProperties { get; set; }

        public IDictionary<string, string> AddedPreparedStatements { get; set; }

        public HashSet<string> DeallocatedPreparedStatements { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TransactionId StartedTransactionId { get; set; }

        public bool ClearTransactionId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string UpdateType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public StageInfo OutputStage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FailureInfo FailureInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorType ErrorType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ErrorCode ErrorCode { get; set; }

        public bool FinalQueryInfo { get; set; }

        public HashSet<Input> Inputs { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Output Output { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FlattenedPlan Plan { get; set; }

        public bool CompleteInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string ResourceGroupName { get; set; }

        #endregion


        #region Public Methods

        public override string ToString()
        {
            return $"QueryInfo {{queryId={this.QueryId}, state={this.State}, fieldNames=[{String.Join(",", this.FieldNames)}]}}";
        }

        public bool IsCompleteInfo()
        {
            return this.CompleteInfo;
        }

        #endregion
    }
}