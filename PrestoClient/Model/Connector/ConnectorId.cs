using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Connector
{
    /// <summary>
    /// From com.facebook.presto.connector.ConnectorId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class ConnectorId
    {
        #region Private Fields

        private static readonly string INFORMATION_SCHEMA_CONNECTOR_PREFIX = "$info_schema@";
        private static readonly string SYSTEM_TABLES_CONNECTOR_PREFIX = "$system@";

        #endregion

        #region Public Properties

        public string CatalogName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ConnectorId(string catalogName)
        {
            if (String.IsNullOrEmpty(catalogName))
            {
                throw new ArgumentNullException("catalogName", "CatalogName cannot be null or empty.");
            }

            this.CatalogName = catalogName;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.CatalogName;
        }

        public static bool IsInternalSystemConnector(ConnectorId connectorId)
        {
            return connectorId.CatalogName.StartsWith(SYSTEM_TABLES_CONNECTOR_PREFIX) ||
                    connectorId.CatalogName.StartsWith(INFORMATION_SCHEMA_CONNECTOR_PREFIX);
        }

        public static ConnectorId CreateInformationSchemaConnectorId(ConnectorId connectorId)
        {
            return new ConnectorId(INFORMATION_SCHEMA_CONNECTOR_PREFIX + connectorId.CatalogName);
        }

        public static ConnectorId CreateSystemTablesConnectorId(ConnectorId connectorId)
        {
            return new ConnectorId(SYSTEM_TABLES_CONNECTOR_PREFIX + connectorId.CatalogName);
        }

        #endregion
    }
}
