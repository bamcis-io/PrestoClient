using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.DeleteNode.java
    /// </summary>
    public class DeleteNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public DeleteHandle Target { get; }

        public Symbol RowId { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DeleteNode(PlanNodeId id, PlanNode source, DeleteHandle target, Symbol rowId, IEnumerable<Symbol> outputs) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.Target = target ?? throw new ArgumentNullException("target");
            this.RowId = rowId ?? throw new ArgumentNullException("rowId");
            this.Outputs = outputs ?? throw new ArgumentNullException("outputs");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Outputs;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
