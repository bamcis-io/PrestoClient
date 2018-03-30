using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.DistinctLimitNode.java
    /// </summary>
    public class DistinctLimitNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public long Limit { get; }

        public bool Partial { get; }

        public IEnumerable<Symbol> DistinctSymbols { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DistinctLimitNode(PlanNodeId id, PlanNode source, long limit, bool partial, IEnumerable<Symbol> distinctSymbols, Symbol hashSymbol) : base(id)
        {
            ParameterCheck.OutOfRange(limit >= 0, "The limit cannot be less than zero.");

            this.Source = source ?? throw new ArgumentNullException("source");
            this.Limit = limit;
            this.Partial = partial;
            this.DistinctSymbols = distinctSymbols;
            this.HashSymbol = hashSymbol;

            if (this.HashSymbol != null && this.DistinctSymbols.Contains(this.HashSymbol))
            {
                throw new ArgumentException("Distinct symbols should not contain hash symbol.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            if (this.HashSymbol != null)
            {
                return this.DistinctSymbols.Concat(new Symbol[] { this.HashSymbol });
            }
            else
            {
                return this.DistinctSymbols;
            }
        }

        #endregion
    }
}
