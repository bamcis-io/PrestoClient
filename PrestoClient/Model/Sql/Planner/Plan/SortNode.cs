using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SortNode.java
    /// </summary>
    public class SortNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public OrderingScheme OrderingScheme { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SortNode(PlanNodeId id, PlanNode source, OrderingScheme orderingScheme) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.OrderingScheme = orderingScheme ?? throw new ArgumentNullException("orderingScheme");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
