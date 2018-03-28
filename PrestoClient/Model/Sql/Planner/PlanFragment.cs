using BAMCIS.PrestoClient.Model.Execution.PlanFlattener;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles;
using BAMCIS.PrestoClient.Model.Sql.Planner;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    public class PlanFragment
    {
        #region Public Properties

        public PlanFragmentId Id { get; set; }

        [JsonConverter(typeof(PlanNodeConverter))]
        public PlanNode Root { get; set; }

        public IDictionary<Symbol, string> Symbols { get; set; }

        /// <summary>
        /// This is supposed to be a PartitioningHandle
        /// </summary>
        public ConnectorHandleWrapper Partitioning { get; set; }

        public IEnumerable<PlanNodeId> PartitionedSources { get; set; }

        public PartitioningScheme PartitioningScheme { get; set; }

        public PipelineExecutionStrategy PipelineExecutionStrategy { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"PlanFragement {{id={this.Id}, partitioning={this.Partitioning}, partitionedSource={this.PartitionedSources}, partitionFunction={this.PartitioningScheme}}}";
        }

        #endregion
    }
}
