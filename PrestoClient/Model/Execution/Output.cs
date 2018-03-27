using BAMCIS.PrestoClient.Model.Connector;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.Output.java
    /// </summary>
    public class Output
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; }

        public string Schema { get; }

        public string Table { get; }

        #endregion

        #region Constructors

        public Output(ConnectorId connectorId, string schema, string table)
        {
            if (String.IsNullOrEmpty(schema))
            {
                throw new ArgumentNullException("schema", "Schema cannot be null or empty.");
            }

            if (String.IsNullOrEmpty(table))
            {
                throw new ArgumentNullException("table", "Table cannot be null or empty.");
            }

            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId", "The connector id cannot be null.");
        }

        #endregion
    }
}
