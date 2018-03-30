using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SetOperationNode.java
    /// </summary>
    public class SetOperationNode : PlanNode
    {
        #region Public Properties

        public IEnumerable<PlanNode> Sources { get; }

        public IEnumerable<KeyValuePair<Symbol, Symbol>> OutputToInputs { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        public SetOperationNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : base(id)
        {
            this.Sources = sources ?? throw new ArgumentNullException("sources");
            this.OutputToInputs = outputToInputs ?? throw new ArgumentNullException("outputToInputs");
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
            return this.Sources;
        }

        #endregion
    }
}
