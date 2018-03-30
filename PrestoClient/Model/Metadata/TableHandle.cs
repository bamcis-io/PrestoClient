using BAMCIS.PrestoClient.Model.Connector;
using BAMCIS.PrestoClient.Model.SPI;
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

        public IConnectorTableHandle ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableHandle(ConnectorId connectorId, IConnectorTableHandle connectorHandle)
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
