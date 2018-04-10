using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.LateralJoinNode.java
    /// </summary>
    public class LateralJoinNode : PlanNode
    {
        #region Public Properties

        public PlanNode Input { get; }

        public PlanNode Subquery { get; }

        /// <summary>
        /// Correlation symbols, returned from input (outer plan) used in subquery (inner plan)
        /// </summary>
        public IEnumerable<Symbol> Correlation { get; }

        public LateralJoinType Type { get; }

        /// <summary>
        /// Used for error reporting in case this ApplyNode is not supported
        /// </summary>
        public Node OriginSubquery { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public LateralJoinNode(PlanNodeId id, PlanNode input, PlanNode subquery, IEnumerable<Symbol> correlation, Node originSubquery) : base(id)
        {
            this.Input = input ?? throw new ArgumentNullException("input");
            this.Subquery = subquery ?? throw new ArgumentNullException("subquery");
            this.Correlation = correlation ?? throw new ArgumentNullException("correlation");
            this.OriginSubquery = originSubquery ?? throw new ArgumentNullException("originSubquery");

            ParameterCheck.Check(this.Correlation.All(x => this.Input.GetOutputSymbols().Contains(x)), "Input does not contain symbol from correlation.");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Input.GetOutputSymbols().Concat(this.Subquery.GetOutputSymbols());
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Input;

            yield return this.Subquery;
        }

        #endregion
    }
}
