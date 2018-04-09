using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.OutputNode.java
    /// </summary>
    public class OutputNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<string> Columns { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public OutputNode(PlanNodeId id, PlanNode source, IEnumerable<string> columns, IEnumerable<Symbol> outputs) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.Columns = columns ?? throw new ArgumentNullException("columns");
            this.Outputs = outputs ?? throw new ArgumentNullException("outputs");

            if (this.Columns.Count() != this.Outputs.Count())
            {
                throw new ArgumentException("Column names and assignments sizes don't match.");
            }
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
