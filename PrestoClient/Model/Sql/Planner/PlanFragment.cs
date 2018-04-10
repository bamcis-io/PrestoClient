using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PlanFragment.java
    /// </summary>
    public class PlanFragment
    {
        #region Public Properties

        public PlanFragmentId Id { get; set; }

        public PlanNode Root { get; set; }

        /// <summary>
        /// TODO: Should be <Symbol, string> Problem with Json.NET
        /// </summary>
        public IDictionary<string, string> Symbols { get; set; }

        public PartitioningHandle Partitioning { get; set; }

        public IEnumerable<PlanNodeId> PartitionedSources { get; set; }

        public PartitioningScheme PartitioningScheme { get; set; }

        public PipelineExecutionStrategy PipelineExecutionStrategy { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Types { get; }

        [JsonIgnore]
        public HashSet<PlanNode> PartionedSourceNodes { get; }

        [JsonIgnore]
        public IEnumerable<RemoteSourceNode> RemoteSourceNodes { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PlanFragment(
            PlanFragmentId id,
            PlanNode root,
            IDictionary<string, string> symbols,
            PartitioningHandle partitioning,
            IEnumerable<PlanNodeId> partitionedSources,
            PartitioningScheme partitioningScheme,
            PipelineExecutionStrategy pipelineExecutionStrategy
            )
        {
            this.Id = id ?? throw new ArgumentNullException("id");
            this.Root = root ?? throw new ArgumentNullException("root");
            this.Symbols = symbols ?? throw new ArgumentNullException("symbols");
            this.Partitioning = partitioning ?? throw new ArgumentNullException("partitioning");
            this.PartitionedSources = partitionedSources ?? throw new ArgumentNullException("partitionedSources");
            this.PipelineExecutionStrategy = pipelineExecutionStrategy;
            this.PartitioningScheme = partitioningScheme ?? throw new ArgumentNullException("partitioningScheme");

            ParameterCheck.Check(this.PartitionedSources.Distinct().Count() == this.PartitionedSources.Count(), "PartitionedSources contains duplicates.");

            this.Types = this.PartitioningScheme.OutputLayout.Select(x => x.ToString());
            // Materialize this during construction
            this.PartionedSourceNodes = new HashSet<PlanNode>(FindSources(this.Root, this.PartitionedSources));
            this.RemoteSourceNodes = FindRemoteSourceNodes(this.Root).ToList();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("id", this.Id)
                .Add("partitioning", this.Partitioning)
                .Add("partitionedSource", this.PartitionedSources)
                .Add("partitionFunction", this.PartitioningScheme)
                .ToString();
        }

        #endregion

        #region Private Methods

        private IEnumerable<PlanNode> FindSources(PlanNode node, IEnumerable<PlanNodeId> nodeIds)
        {
            if (nodeIds.Contains(node.Id))
            {
                yield return node;
            }

            foreach (PlanNode Source in node.GetSources())
            {
                foreach (PlanNode Item in FindSources(Source, nodeIds))
                {
                    yield return Item;
                }
            }
        }

        private static IEnumerable<RemoteSourceNode> FindRemoteSourceNodes(PlanNode node)
        {
            foreach (PlanNode Source in node.GetSources())
            {
                foreach (RemoteSourceNode Item in FindRemoteSourceNodes(Source))
                {
                    yield return Item;
                }
            }

            if (node is RemoteSourceNode)
            {
                yield return (RemoteSourceNode)node;
            }
        }

        #endregion
    }
}
