using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.FilterNode.java
    /// </summary>
    public class FilterNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Predicate { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public FilterNode(PlanNodeId id, PlanNode source, dynamic predicate) : base(id)
        {
            this.Source = source;
            this.Predicate = predicate;
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
