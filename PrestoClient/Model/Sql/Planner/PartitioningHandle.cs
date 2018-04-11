using BAMCIS.PrestoClient.Model.Connector;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PartitioningHandle.java
    /// </summary>
    public class PartitioningHandle
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public ConnectorId ConnectorId { get; }

        /// <summary>
        /// This should be an IConnectorTransactionHandle, but all of the 
        /// classes that implement this interface have not been created
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public dynamic TransactionHandle { get; }

        /// <summary>
        /// This should be an IConnectorHandle, but all of the classes
        /// that implement this interface have not been created
        /// </summary>
        public dynamic ConnectorHandle { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PartitioningHandle(ConnectorId connectorId, dynamic transactionHandle, dynamic connectorHandle)
        {
            this.ConnectorId = connectorId;
            this.TransactionHandle = transactionHandle;
            this.ConnectorHandle = connectorHandle ?? throw new ArgumentNullException("connectorHandle");

            // If connector id is not null, then it will check the second condition
            ParameterCheck.Check(this.ConnectorId == null || this.TransactionHandle != null, "Transaction handle required when connector id is present.");
        }

        #endregion

        #region Public Methods

        // Do not include these currently since the ConnectorHandle class is dynamic and won't provide
        // any implementation for these methods
        /*
        public bool IsSingleNode()
        {
            return this.ConnectorHandle.IsSingleNode();
        }

        public bool IsCoordinatorOnly()
        {
            return this.ConnectorHandle.IsCoordinatorOnly();
        }
        */

        public override string ToString()
        {
            if (this.ConnectorId != null)
            {
                return $"{this.ConnectorId.ToString()}:{this.ConnectorHandle.ToString()}";
            }
            else
            {
                return this.ConnectorHandle.ToString();
            }
        }

        #endregion
    }
}
