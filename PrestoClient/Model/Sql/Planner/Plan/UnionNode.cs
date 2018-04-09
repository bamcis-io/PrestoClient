using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.UnionNode.java
    /// </summary>
    public class UnionNode : SetOperationNode
    {
        #region Public Properties

        #endregion

        #region Constructors

        [JsonConstructor]
        public UnionNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : base(id, sources, outputToInputs, outputs)
        { }

        #endregion
    }
}
