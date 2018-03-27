using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class QueryError
    {
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SqlState { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorName { get; set; }

        public string ErrorType { get; set; }

        public ErrorLocation ErrorLocation { get; set; }

        public FailureInfo FailureInfo { get; set; }
    }
}