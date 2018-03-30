using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExplainAnalyzeNode.java
    /// </summary>
    public class ExplainAnalyzeNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Symbol OutputSymbol { get; }

        public bool Verbose { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ExplainAnalyzeNode(PlanNodeId id, PlanNode source, Symbol outputSymbol, bool verbose) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.OutputSymbol = outputSymbol ?? throw new ArgumentNullException("outputSymbol");
            this.Verbose = verbose;
        }

        #endregion

        #region Public Properties

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            yield return this.OutputSymbol;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
