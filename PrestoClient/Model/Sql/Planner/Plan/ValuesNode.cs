using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ValuesNode.java
    /// </summary>
    public class ValuesNode : PlanNode
    {
        #region Public Properties

        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public IEnumerable<List<dynamic>> Rows { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ValuesNode(PlanNodeId id, IEnumerable<Symbol> outputSymbols, IEnumerable<List<dynamic>> rows) : base(id)
        {
            this.OutputSymbols = outputSymbols;
            this.Rows = rows;

            int OutputSymbolsSize = outputSymbols.Count();

            foreach (List<dynamic> Row in this.Rows)
            {
                ParameterCheck.OutOfRange(Row.Count == OutputSymbolsSize || Row.Count == 0, $"Expected row to have {OutputSymbolsSize} values, but row has {Row.Count} values.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return new PlanNode[0];
        }

        #endregion
    }
}
