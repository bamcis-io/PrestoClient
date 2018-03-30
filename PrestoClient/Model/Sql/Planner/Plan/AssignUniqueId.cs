using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.AssignUniqueId.java
    /// </summary>
    public class AssignUniqueId : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Symbol IdColumn { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public AssignUniqueId(PlanNodeId id, PlanNode source, Symbol idColumn) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.IdColumn = idColumn ?? throw new ArgumentNullException("idColumn");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols().Concat(new Symbol[] { this.IdColumn });
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
