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

        public ConnectorId ConnectorId { get; }

        /// <summary>
        /// TODO: Supposed to be an ITableConnectorHandle
        /// </summary>
        public dynamic ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableHandle(ConnectorId connectorId, dynamic connectorHandle)
        {
            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.ConnectorId}:{this.ConnectorHandle}";
        }

        #endregion
    }
}
