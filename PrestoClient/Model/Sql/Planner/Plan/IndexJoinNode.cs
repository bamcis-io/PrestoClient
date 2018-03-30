using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IndexJoinNode.java
    /// </summary>
    public class IndexJoinNode : PlanNode
    {
        #region Public Properties

        public IndexJoinNodeType Type { get; }

        public PlanNode ProbeSource { get; }

        public PlanNode IndexSource { get; }

        public IEnumerable<EquiJoinClause> Criteria { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol ProbeHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol IndexHashSymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public IndexJoinNode(
            PlanNodeId id,
            IndexJoinNodeType type,
            PlanNode probeSource,
            PlanNode indexSource,
            IEnumerable<EquiJoinClause> criteria,
            Symbol probeHashSymbol,
            Symbol indexHashSymbol
            ) : base(id)
        {
            this.Type = type;
            this.ProbeSource = probeSource ?? throw new ArgumentNullException("probeSource");
            this.IndexSource = indexSource ?? throw new ArgumentNullException("indexSource");
            this.Criteria = criteria ?? throw new ArgumentNullException("criteria");
            this.ProbeHashSymbol = probeHashSymbol;
            this.IndexHashSymbol = indexHashSymbol;    
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.ProbeSource.GetOutputSymbols().Concat(this.IndexSource.GetOutputSymbols());
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.ProbeSource;
            yield return this.IndexSource;
        }

        #endregion

        #region Internal Classes

        public class EquiJoinClause
        {
            #region Public Properties

            public Symbol Probe { get; }

            public Symbol Index { get; }

            #endregion

            #region Constructors

            public EquiJoinClause(Symbol probe, Symbol index)
            {
                this.Probe = probe ?? throw new ArgumentNullException("probe");
                this.Index = index ?? throw new ArgumentNullException("index");
            }

            #endregion
        }

        #endregion
    }
}
