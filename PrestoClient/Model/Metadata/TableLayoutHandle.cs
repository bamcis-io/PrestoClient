using BAMCIS.PrestoClient.Model.Connector;
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

        /// <summary>
        /// TODO: Supposed to be IConnectorTransactionHandle
        /// </summary>
        public dynamic TransactionHandle { get; }

        /// <summary>
        /// TODO: Supposed to be IConnectorTableLayoutHandle
        /// </summary>
        public dynamic ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableLayoutHandle(ConnectorId connectorId, dynamic transactionHandle, dynamic connectorHandle)
        {
            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId");
            this.TransactionHandle = transactionHandle ?? throw new ArgumentNullException("transactionHandle");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion
    }
}
