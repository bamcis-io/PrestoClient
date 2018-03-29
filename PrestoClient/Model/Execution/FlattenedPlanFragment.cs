using BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles;
using BAMCIS.PrestoClient.Model.Sql.Planner;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.PlanFlattener.java (internal class FlattendedPlanFragment)
    /// </summary>
    public class FlattenedPlanFragment
    {
        public string Id { get; set; }

        public string TextPlan { get; set; }

        public PlanNode Tree { get; set; }

        /// <summary>
        /// TODO: This is supposed to be flattened node, but 
        /// the type isn't built out completely yet
        /// </summary>
        public IEnumerable<PlanNode> Nodes { get; set; }

        /// <summary>
        /// TODO: Should be <Symbol, string> Problem with Json.NET
        /// </summary>
        public IDictionary<string, string> Symbols { get; set; }

        public ConnectorHandleWrapper Partitioning { get; set; }

        public IEnumerable<PlanNodeId> PartitionedSources { get; set; }

        public IEnumerable<string> Types { get; set; }

        public IEnumerable<PlanNode> PartitionedSourceNodes { get; set; }

        public IEnumerable<RemoteSourceNode> RemoteSourceNodes { get; set; }

        public PartitioningScheme PartitioningScheme { get; set; }
    }
}
