using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExceptNode.java
    /// </summary>
    public class ExceptNode : SetOperationNode
    {
        #region Public Properties

        #endregion

        #region Constructors

        [JsonConstructor]
        public ExceptNode(PlanNodeId id, IEnumerable<PlanNode> sources, IEnumerable<KeyValuePair<Symbol, Symbol>> outputToInputs, IEnumerable<Symbol> outputs) : base(id, sources, outputToInputs, outputs)
        {
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
