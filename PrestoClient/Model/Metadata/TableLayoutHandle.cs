using BAMCIS.PrestoClient.Model.Connector;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.SPI.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.TableLayoutHandle.java
    /// </summary>
    public sealed class TableLayoutHandle
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; }

        public IConnectorTransactionHandle TransactionHandle { get; }

        public IConnectorTableLayoutHandle ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableLayoutHandle(ConnectorId connectorId, IConnectorTransactionHandle transactionHandle, IConnectorTableLayoutHandle connectorHandle)
        {
            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId");
            this.TransactionHandle = transactionHandle ?? throw new ArgumentNullException("transactionHandle");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion
    }
}
