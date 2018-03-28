using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.QueryError.java
    /// </summary>
    public class QueryError
    {
        #region Public Properties

        public string Message { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SqlState { get; }

        public int ErrorCode { get; }

        public string ErrorName { get; }

        public string ErrorType { get; }

        public ErrorLocation ErrorLocation { get; }

        public FailureInfo FailureInfo { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryError(string message, string sqlState, int errorCode, string errorName, string errorType, ErrorLocation errorLocation, FailureInfo failureInfo)
        {
            this.Message = message;
            this.SqlState = sqlState;
            this.ErrorCode = errorCode;
            this.ErrorName = errorName;
            this.ErrorType = errorType;
            this.ErrorLocation = errorLocation;
            this.FailureInfo = failureInfo;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("message", this.Message)
                .Add("sqlState", this.SqlState)
                .Add("errorCode", this.ErrorCode)
                .Add("errorName", this.ErrorName)
                .Add("errorType", this.ErrorType)
                .Add("errorLocation", this.ErrorLocation)
                .Add("failureInfo", this.FailureInfo)
                .ToString();
        }

        #endregion
    }
}