using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.RowNumberNode.java
    /// </summary>
    public class RowNumberNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<Symbol> PartitionBy { get; }

        public Symbol RowNumberSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public int MaxRowCountPerPartition { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public RowNumberNode(PlanNodeId id, PlanNode source, IEnumerable<Symbol> partitionBy, Symbol rowNumberSymbol, int maxRowCountPerPartition, Symbol hashSymbol) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.PartitionBy = partitionBy ?? throw new ArgumentNullException("partitionBy");
            this.RowNumberSymbol = rowNumberSymbol ?? throw new ArgumentNullException("rowNumberSymbol");
            this.MaxRowCountPerPartition = maxRowCountPerPartition;
            this.HashSymbol = hashSymbol; // ?? throw new ArgumentNullException("hashSymbol");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols().Concat(new Symbol[] { this.RowNumberSymbol });
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
