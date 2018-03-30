using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExchangeNode.java
    /// </summary>
    public class ExchangeNode : PlanNode
    {
        #region Public Properties

        public ExchangeType Type { get; }

        public ExchangeScope Scope { get; }
  
        public IEnumerable<PlanNode> Sources { get; }

        public PartitioningScheme PartitioningScheme { get; }

        public IEnumerable<List<Symbol>> Inputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ExchangeNode(PlanNodeId id, ExchangeType type, ExchangeScope scope, PartitioningScheme partitioningScheme, IEnumerable<PlanNode> sources, IEnumerable<List<Symbol>> inputs) : base(id)
        {
            this.Type = type;
            this.Scope = scope;
            this.PartitioningScheme = partitioningScheme ?? throw new ArgumentNullException("partitioningScheme");
            this.Sources = sources ?? throw new ArgumentNullException("sources");
            this.Inputs = inputs ?? throw new ArgumentNullException("inputs");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            return this.Sources;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.PartitioningScheme.OutputLayout;
        }

        #endregion
    }
}
