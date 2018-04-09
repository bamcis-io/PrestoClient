using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.LimitNode.java
    /// </summary>
    public class LimitNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public long Count { get; }

        public bool Partial { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public LimitNode(PlanNodeId id, PlanNode source, long count, bool partial) : base(id)
        {
            ParameterCheck.OutOfRange(count >= 0, "count", "Count cannot be less than 0.");

            this.Source = source ?? throw new ArgumentNullException("source");
            this.Count = count;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols();
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
