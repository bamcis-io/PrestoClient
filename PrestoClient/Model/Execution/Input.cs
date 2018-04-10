using BAMCIS.PrestoClient.Model.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Input.java
    /// </summary>
    public class Input
    {
        #region Public Properties

        public ConnectorId ConnectorId { get;  }

        public string Schema { get;  }

        public string Table { get; }

        /// <summary>
        /// Not included in JSON serialization if not present
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public PrestoQueryConnectorInfo ConnectorInfo { get; set; }

        public IEnumerable<Column> Columns { get; }

        #endregion

        #region Constructors

        public Input(ConnectorId connectorId, string schema, string table, IEnumerable<Column> columns, PrestoQueryConnectorInfo connectorInfo = null)
        {
            if (String.IsNullOrEmpty(schema))
            {
                throw new ArgumentNullException("schema", "The schema cannot be null or empty.");
            }

            if (String.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException("table", "The table cannot be null or empty.");
            }

            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId", "The connector id cannot be null.");
            this.Columns = columns ?? throw new ArgumentNullException("columns", "The columns cannot be null.");
            this.ConnectorInfo = connectorInfo;
        }

        #endregion

        #region Child Classes

        public class PrestoQueryConnectorInfo
        {
            public IEnumerable<string> PartitionIds { get; set; }
        }

        #endregion
    }
}