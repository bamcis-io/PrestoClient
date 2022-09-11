using BAMCIS.PrestoClient.Model.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TableHandle.java
    /// </summary>
    public class TableHandle
    {
        #region Public Properties

        public CatalogHandle catalogHandle { get; }

        /// <summary>
        /// TODO: Supposed to be an IConnectorTableHandle
        /// </summary>
        public dynamic ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableHandle(CatalogHandle catalogHandle, dynamic connectorHandle)
        {
            this.catalogHandle = catalogHandle ?? throw new ArgumentNullException("catalogHandle");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.catalogHandle}:{this.ConnectorHandle}";
        }

        #endregion
    }
}
