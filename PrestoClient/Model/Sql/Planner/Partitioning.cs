using BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Partitioning.java
    /// </summary>
    public class Partitioning
    {
        /// <summary>
        /// This is really supposed to be a PartitioningHandle
        /// </summary>
        public ConnectorHandleWrapper Handle { get; set; }

        public IEnumerable<ArgumentBinding> Arguments { get; set; }
    }
}