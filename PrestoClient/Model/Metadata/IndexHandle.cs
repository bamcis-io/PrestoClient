using BAMCIS.PrestoClient.Model.Connector;
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

        public CatalogHandle CatalogHandle { get; }

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
        public IndexHandle(CatalogHandle catalogHandle, dynamic transactionHandle, dynamic connectorHandle)
        {
            this.CatalogHandle = catalogHandle ?? throw new ArgumentNullException("catalogHandle");
            this.TransactionHandle = transactionHandle ?? throw new ArgumentNullException("transactionHandle");
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.CatalogHandle}:{this.TransactionHandle}:{this.ConnectorHandle}";
        }

        #endregion
    }
}
