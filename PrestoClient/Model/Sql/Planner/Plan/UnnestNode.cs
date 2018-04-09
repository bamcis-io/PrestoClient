using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.UnnestNode.java
    /// </summary>
    public class UnnestNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<Symbol> ReplicateSymbols { get; }

        /// <summary>
        /// TODO: Key should be Symbol
        /// </summary>
        public IDictionary<string, List<Symbol>> UnnestSymbols { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol OrdinalitySymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public UnnestNode(PlanNodeId id, PlanNode source, IEnumerable<Symbol> replicateSymbols, IDictionary<string, List<Symbol>> unnestSymbols, Symbol ordinalitySymbol) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.ReplicateSymbols = replicateSymbols ?? throw new ArgumentNullException("replicateSymbols");
            this.UnnestSymbols = unnestSymbols ?? throw new ArgumentNullException("unnestSymbols");
            this.OrdinalitySymbol = ordinalitySymbol;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            IEnumerable <Symbol> Temp = this.ReplicateSymbols.Concat(this.UnnestSymbols.Keys.Select(x => new Symbol(x)));

            if (this.OrdinalitySymbol != null)
            {
                Temp.Concat(new Symbol[] { this.OrdinalitySymbol });
            }

            return Temp;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
