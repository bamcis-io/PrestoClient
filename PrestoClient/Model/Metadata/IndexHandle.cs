using BAMCIS.PrestoClient.Model.Connector;
using BAMCIS.PrestoClient.Model.SPI;
using BAMCIS.PrestoClient.Model.SPI.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.IndexHandle.java
    /// </summary>
    public sealed class IndexHandle
    {
        #region Public Properties

        public ConnectorId ConnectorId { get; }

        /// <summary>
        /// TODO: Supposed to be IConnectorTransactionHandle
        /// </summary>
        public dynamic TransactionHandle { get; }

        /// <summary>
        /// TODO: Supposed to be IConnectorIndexHandle
        /// </summary>
        public dynamic ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public IndexHandle(ConnectorId connectorId, dynamic transactionHandle, dynamic connectorHandle)
        {
            this.ConnectorId = connectorId ?? throw new ArgumentNullException("connectorId");
            this.TransactionHandle = transactionHandle ?? throw new ArgumentNullException("transactionHandle");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.ConnectorId}:{this.TransactionHandle}:{this.ConnectorHandle}";
        }

        #endregion
    }
}
