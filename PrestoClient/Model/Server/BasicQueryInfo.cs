using BAMCIS.PrestoClient.Model.Execution;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.SPI.Memory;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// Lightweight version of QueryInfo. Parts of the web UI depend on the fields
    /// being named consistently across these classes.
    /// From com.facebook.presto.server.BasicQueryInfo.java
    /// </summary>
    public class BasicQueryInfo
    {
        #region Public Properties

        public QueryId QueryId { get; }

        public SessionRepresentation Session { get;  }

        public QueryState State { get;  }

        public MemoryPoolId MemoryPool { get; }

        public bool Scheduled { get; }

        public Uri Self { get; }

        public string Query { get; }

        public BasicQueryStats QueryStats { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorType ErrorType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorCode ErrorCode { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public BasicQueryInfo(
            QueryId queryId,
            SessionRepresentation session,
            QueryState state,
            MemoryPoolId memoryPool,
            bool scheduled,
            Uri self,
            string query,
            BasicQueryStats queryStats,
            ErrorType errorType,
            ErrorCode errorCode
            )
        {
            ParameterCheck.NotNullOrEmpty(query, "query");

            this.QueryId = queryId ?? throw new ArgumentNullException("queryId");
            this.Session = session ?? throw new ArgumentNullException("session");
            this.State = state;
            this.MemoryPool = memoryPool;
            this.ErrorType = errorType;
            this.ErrorCode = errorCode;
            this.Scheduled = scheduled;
            this.Self = self ?? throw new ArgumentNullException("self");
            this.Query = query;
            this.QueryStats = queryStats ?? throw new ArgumentNullException("queryStats");
        }

        public BasicQueryInfo(QueryInfo queryInfo) : 
            this(queryInfo.QueryId, queryInfo.Session, queryInfo.State, queryInfo.MemoryPool, queryInfo.Scheduled, queryInfo.Self,
                queryInfo.Query, new BasicQueryStats(queryInfo.QueryStats), queryInfo.ErrorType, queryInfo.ErrorCode)
        {            
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("queryId", this.QueryId)
                .Add("state", this.State.ToString())
                .ToString();
        }

        #endregion
    }
}