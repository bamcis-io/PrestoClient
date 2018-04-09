using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ProjectNode.java
    /// </summary>
    public class ProjectNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Assignments Assignments { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ProjectNode(PlanNodeId id, PlanNode source, Assignments assignments) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.Assignments = assignments ?? throw new ArgumentNullException("assignments");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Assignments.GetOutputs();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
