using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.MarkDistinctOperator.java
    /// </summary>
    public class MarkDistinctNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Symbol MarkerSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        public IEnumerable<Symbol> DistinctSymbols { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public MarkDistinctNode(PlanNodeId id, PlanNode source, Symbol markerSymbol, IEnumerable<Symbol> distinctSymbols, Symbol hashSymbol) : base(id)
        {
            this.Source = source;
            this.MarkerSymbol = markerSymbol;
            this.HashSymbol = hashSymbol;
            this.DistinctSymbols = distinctSymbols ?? throw new ArgumentNullException("distinctSymbols");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols().Concat(new Symbol[] { this.MarkerSymbol });
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
