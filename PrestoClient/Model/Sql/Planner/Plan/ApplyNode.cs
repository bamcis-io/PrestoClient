using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ApplyNode.java
    /// </summary>
    public class ApplyNode : PlanNode
    {
        #region Public Properties

        public PlanNode Input { get; }

        public PlanNode SubQuery { get; }

        public IEnumerable<Symbol> Correlation { get; }

        public Assignments SubqueryAssignments { get; }

        public Node OriginSubquery { get; }

        public IEnumerable<Symbol> OutputSymbols
        {
            get
            {
                return this.Input.GetOutputSymbols().Concat(this.SubqueryAssignments.GetOutputs());
            }
        }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ApplyNode(PlanNodeId id, PlanNode input, PlanNode subquery, Assignments subqueryAssignments, IEnumerable<Symbol> correlation, Node originSubquery) : base(id)
        {
            this.Input = input ?? throw new ArgumentNullException("input");
            this.SubQuery = subquery ?? throw new ArgumentNullException("subquery");
            this.SubqueryAssignments = subqueryAssignments ?? throw new ArgumentNullException("subqueryAssignments");
            this.Correlation = correlation ?? throw new ArgumentNullException("correlation");
            this.OriginSubquery = originSubquery ?? throw new ArgumentNullException("originSubquery");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Input;

            yield return this.SubQuery;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Input.GetOutputSymbols().Concat(this.SubqueryAssignments.GetOutputs());
        }

        #endregion
    }
}
