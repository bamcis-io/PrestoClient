using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SampleNode.java
    /// </summary>
    public class SampleNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public double SampleRatio { get; }

        public SampleNodeType SampleType { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public SampleNode(PlanNodeId id, PlanNode source, double sampleRatio, SampleNodeType sampleType) : base(id)
        {
            ParameterCheck.OutOfRange(sampleRatio >= 0.0, "sampleRatio", "Sample ratio cannot be less than zero.");
            ParameterCheck.OutOfRange(sampleRatio <= 1.0, "sampleRatio", "Sample ratio cannot be greater than 1.");

            this.SampleType = sampleType;
            this.Source = source ?? throw new ArgumentNullException("source");
            this.SampleRatio = sampleRatio;
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
