using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.TableFinishNode.java
    /// </summary>
    public class TableFinishNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }
        
        public WriterTarget Target { get; }

        public IEnumerable<Symbol> Outputs { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TableFinishNode(PlanNodeId id, PlanNode source, WriterTarget target, IEnumerable<Symbol> outputs) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.Target = target;
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
