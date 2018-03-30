using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.EnforceSingleRowNode.java
    /// </summary>
    public class EnforceSingleRowNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public EnforceSingleRowNode(PlanNodeId id, PlanNode source) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols();
        }

        #endregion
    }
}
