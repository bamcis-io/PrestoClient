using BAMCIS.PrestoClient.Model.Sql.Planner;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.PlanFlattener.java (internal class FlattendedPlanFragment)
    /// </summary>
    [JsonConverter(typeof(FlattenedPlanFragmentConverter))]
    public class FlattenedPlanFragment
    {
        #region Public Properties

        public PlanFragmentId Id
        {
            get
            {
                return this.Fragment.Id;
            }
        }

        public string TextPlan { get; }

        public PlanNode Tree
        {
            get
            {
                return this.Fragment.Root;
            }
        }

        public IEnumerable<FlattenedNode> Nodes { get; }

        /// <summary>
        /// TODO: Should be <Symbol, Type> Problem with Json.NET
        /// </summary>
        public IDictionary<string, string> Symbols
        {
            get
            {
                return this.Fragment.Symbols;
            }
        }

        public PartitioningHandle Partitioning
        {
            get
            {
                return this.Fragment.Partitioning;
            }
        }

        public IEnumerable<PlanNodeId> PartitionedSources
        {
            get
            {
                return this.Fragment.PartitionedSources;
            }
        }

        public IEnumerable<string> Types
        {
            get
            {
                return this.Fragment.Types;
            }
        }

        public HashSet<PlanNode> PartitionedSourceNodes
        {
            get
            {
                return this.Fragment.PartionedSourceNodes;
            }
        }

        public IEnumerable<RemoteSourceNode> RemoteSourceNodes
        {
            get
            {
                return this.Fragment.RemoteSourceNodes;
            }
        }

        public PartitioningScheme PartitioningScheme
        {
            get
            {
                return this.Fragment.PartitioningScheme;
            }
        }

        #endregion

        #region Private Properties

        private PlanFragment Fragment { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public FlattenedPlanFragment(string textPlan, PlanFragment fragment, IEnumerable<FlattenedNode> nodes)
        {
            if (string.IsNullOrEmpty(textPlan))
            {
                throw new ArgumentNullException("textPlan");
            }

            this.TextPlan = textPlan;
            this.Fragment = fragment ?? throw new ArgumentNullException("fragment");
            this.Nodes = nodes ?? throw new ArgumentNullException("nodes");
        }

        #endregion 
    }
}
