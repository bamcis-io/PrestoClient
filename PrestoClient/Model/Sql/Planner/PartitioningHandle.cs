using BAMCIS.PrestoClient.Model.Connector;
using BAMCIS.PrestoClient.Model.SPI.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PartitioningHandle.java
    /// 
    /// This is not used because no classes that implement the IConnectorTransactionHandle or IConnectorPartitioningHandle 
    /// have been created yet
    /// </summary>
    public class PartitioningHandle : IConnectorPartitioningHandle
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ConnectorId ConnectorId { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IConnectorTransactionHandle TransactionHandle { get; }

        public IConnectorPartitioningHandle ConnectorHandle { get; }

        #endregion

        #region Constructors

        public PartitioningHandle(IConnectorPartitioningHandle connectorHandle, ConnectorId connectorId = null, IConnectorTransactionHandle transactionHandle = null)
        {
            if (connectorId != null && transactionHandle == null)
            {
                throw new ArgumentException("Transaction handle is required when connector id is provided.");
            }

            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle", "The connector handle cannot be null.");
            this.ConnectorId = connectorId;
            this.TransactionHandle = transactionHandle;
        }

        #endregion

        #region Public Methods

        public bool IsSingleNode()
        {
            return this.ConnectorHandle.IsSingleNode();
        }

        public bool IsCoordinatorOnly()
        {
            return this.ConnectorHandle.IsCoordinatorOnly();
        }

        public override string ToString()
        {
            if (this.ConnectorId != null)
            {
                return $"{this.ConnectorId.ToString()}:{this.ConnectorHandle}";
            }
            else
            {
                return this.ConnectorHandle.ToString();
            }
        }

        #endregion
    }
}
