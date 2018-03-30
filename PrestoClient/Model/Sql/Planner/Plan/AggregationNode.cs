using BAMCIS.PrestoClient.Model.Metadata;
using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.AggregationNode.java
    /// </summary>
    public class AggregationNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        /// <summary>
        /// TODO: Should be IDictionary<Symbol, Aggregation> but symbol doesn't work as key
        /// </summary>
        public IDictionary<string, Aggregation> Aggregations { get; }

        public IEnumerable<List<Symbol>> GroupingSets { get; }

        public Step Step { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol GroupIdSymbol { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public AggregationNode(
            PlanNodeId id, 
            PlanNode source, 
            IDictionary<string, Aggregation> aggregations, 
            IEnumerable<List<Symbol>> groupingSets, 
            Step step, 
            Symbol hashSymbol, 
            Symbol groupIdSymbol) : base(id)
        {
            this.Source = source;
            this.Aggregations = aggregations;

            if (groupingSets == null)
            {
                throw new ArgumentNullException("groupingSets");
            }

            if (!groupingSets.Any())
            {
                throw new ArgumentException("Grouping sets cannot be empty.", "groupingSets");
            }

            this.GroupingSets = groupingSets;
            this.Step = step;
            this.HashSymbol = hashSymbol;
            this.GroupIdSymbol = groupIdSymbol;

            this.Outputs = this.GetGroupingKeys().Concat(this.Aggregations.Keys.Select(x => new Symbol(x)));

            if (this.HashSymbol != null)
            {
                this.Outputs = this.Outputs.Concat(new Symbol[] { this.HashSymbol });
            }
        }

        #endregion

        #region Public Methods

        public IEnumerable<Symbol> GetGroupingKeys()
        {
            IEnumerable<Symbol> Temp = this.GroupingSets.SelectMany(x => x).Distinct();

            if (this.GroupIdSymbol != null)
            {
                return Temp.Concat(new Symbol[] { this.GroupIdSymbol });
            }
            else
            {
                return Temp;
            }
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion

        #region Internal Classes

        public class Aggregation
        {
            #region Public Properties

            public FunctionCall Call { get; }

            public Signature Signature { get; }

            public Symbol Mask { get; }

            #endregion

            #region Constructors

            public Aggregation(FunctionCall call, Signature signature, Symbol mask)
            {
                this.Call = call;
                this.Signature = signature;
                this.Mask = mask;
            }

            #endregion
        }
        #endregion
    }
}
