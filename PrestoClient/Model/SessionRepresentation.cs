using BAMCIS.PrestoClient.Model.Connector;
using BAMCIS.PrestoClient.Model.SPI.Type;
using BAMCIS.PrestoClient.Model.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From com.facebook.presto.SessionRepresentation.java
    /// </summary>
    public class SessionRepresentation
    {
        public string QueryId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TransactionId TransactionId { get; set; }

        public bool ClientTransactionSupport { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string User { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string Principal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string Source { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string Catalog { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string Schema { get; set; }

        public TimeZoneKey TimeZoneKey { get; set; }

        public string Locale { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string RemoteUserAddress { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string UserAgent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public string ClientInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public HashSet<string> ClientTags { get; set; }


        public Int64 StartTime { get; set; }


        public IDictionary<string, string> SystemProperties { get; set; }


        public IDictionary<CatalogHandle, IDictionary<string, string>> CatalogProperties { get; set; }


        public IDictionary<string, string> PreparedStatements { get; set; }
    }
}
