using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Connector
{
    /// <summary>
    /// From com.facebook.presto.connector.CatalogHandle.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class CatalogHandle
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
        public CatalogHandle(string catalogName)
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

        public static bool IsInternalSystemConnector(CatalogHandle catalogHandle)
        {
            return catalogHandle.CatalogName.StartsWith(SYSTEM_TABLES_CONNECTOR_PREFIX) ||
                    catalogHandle.CatalogName.StartsWith(INFORMATION_SCHEMA_CONNECTOR_PREFIX);
        }

        public static CatalogHandle CreateInformationSchemaConnectorId(CatalogHandle catalogHandle)
        {
            return new CatalogHandle(INFORMATION_SCHEMA_CONNECTOR_PREFIX + catalogHandle.CatalogName);
        }

        public static CatalogHandle CreateSystemTablesConnectorId(CatalogHandle catalogHandle)
        {
            return new CatalogHandle(SYSTEM_TABLES_CONNECTOR_PREFIX + catalogHandle.CatalogName);
        }

        #endregion
    }
}
