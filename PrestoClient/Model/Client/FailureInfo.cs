using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.FailureInfo.java
    /// </summary>
    public class FailureInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string Type { get; set; }

        public string Message { get; set; }

        public FailureInfo Cause { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<FailureInfo> Suppressed { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Stack { get; set; }

        public ErrorLocation ErrorLocation { get; set; }
    }
}