using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SemiJoinNode.java
    /// </summary>
    public class SemiJoinNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public PlanNode FilteringSource { get; }

        public Symbol SourceJoinSymbol { get; }

        public Symbol FilteringSourceJoinSymbol { get; }

        public Symbol SemiJoinOutput { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol SourceHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol FilteringSourceHashSymbol { get; }

        public DistributionType DistributionType { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SemiJoinNode(
            PlanNodeId id, 
            PlanNode source, 
            PlanNode filteringSource, 
            Symbol sourceJoinSymbol, 
            Symbol filteringSourceJoinSymbol, 
            Symbol semiJoinOutput,
            Symbol sourceHashSymbol,
            Symbol filteringSourceHashSymbol,
            DistributionType distributionType
            ) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.FilteringSource = filteringSource ?? throw new ArgumentNullException("filteringSource");
            this.SourceJoinSymbol = sourceJoinSymbol ?? throw new ArgumentNullException("sourceJoinSymbol");
            this.FilteringSourceJoinSymbol = filteringSourceJoinSymbol ?? throw new ArgumentNullException("filteringSourceJoinSymbol");
            this.SemiJoinOutput = semiJoinOutput ?? throw new ArgumentNullException("semiJoinOutput");
            this.SourceHashSymbol = sourceHashSymbol;
            this.FilteringSourceHashSymbol = filteringSourceHashSymbol;
            this.DistributionType = distributionType;

            ParameterCheck.Check(this.Source.GetOutputSymbols().Contains(this.SourceJoinSymbol), "Source does not contain join symbol.");
            ParameterCheck.Check(this.FilteringSource.GetOutputSymbols().Contains(this.FilteringSourceJoinSymbol), "Filtering source does not contain filtering join symbol.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols().Concat(new Symbol[] { this.SemiJoinOutput });
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;

            yield return this.FilteringSource;
        }

        #endregion
    }
}
