using BAMCIS.PrestoClient.Model.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.MetadataDeleteNode.java
    /// </summary>
    public class MetadataDeleteNode : PlanNode
    {
        #region Public Properties

        public DeleteHandle Target { get; }

        public Symbol Output { get; }

        public TableLayoutHandle TableLayout { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public MetadataDeleteNode(PlanNodeId id, DeleteHandle target, Symbol output, TableLayoutHandle tableLayout) : base(id)
        {
            this.Target = target ?? throw new ArgumentNullException("target");
            this.Output = output ?? throw new ArgumentNullException("output");
            this.TableLayout = tableLayout ?? throw new ArgumentNullException("tableLayout");
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            yield return this.Output;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            return new List<PlanNode>();
        }

        #endregion
    }
}
