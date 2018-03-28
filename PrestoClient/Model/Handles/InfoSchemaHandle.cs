using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles
{
    public class InfoSchemaHandle : Handle
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CatalogName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SchemaName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TableName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ColumnName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid TransactionId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InfoSchemaHandle Table { get; set; }

        /// <summary>
        /// The contents of the constraint are dynamic in the serialized json
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, dynamic> Constraint { get; set; }
    }
}