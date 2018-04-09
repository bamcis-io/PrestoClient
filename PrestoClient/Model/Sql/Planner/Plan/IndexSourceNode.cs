using BAMCIS.PrestoClient.Model.Metadata;
using BAMCIS.PrestoClient.Model.SPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.IndexSourceNode.java
    /// </summary>
    public class IndexSourceNode : PlanNode
    {
        #region Public Properties

        public IndexHandle IndexHandle { get; }

        public TableHandle TableHandle { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public TableLayoutHandle TableLayout { get; }

        public HashSet<Symbol> LookupSymbols { get; }

        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Key is supposed to be Symbol, Key is IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> Assignments { get; }

        /// <summary>
        /// TODO: This should be com.facebook.presto.spi.predicate.TupleDomain.java
        /// of IColumnHandle
        /// </summary>
        public IDictionary<string, dynamic> CurrentConstraint { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public IndexSourceNode(
            PlanNodeId id,
            IndexHandle indexHandle,
            TableHandle tableHandle,
            TableLayoutHandle tableLayout,
            HashSet<Symbol> lookupSymbols,
            IEnumerable<Symbol> outputSymbols,
            IDictionary<string, dynamic> assignments,
            IDictionary<string, dynamic> currentConstraint
            ) : base(id)
        {
            this.IndexHandle = indexHandle ?? throw new ArgumentNullException("indexHandle");
            this.TableHandle = tableHandle ?? throw new ArgumentNullException("tableHanlde");
            this.TableLayout = tableLayout;
            this.LookupSymbols = lookupSymbols ?? throw new ArgumentNullException("lookupSymbols");
            this.OutputSymbols = outputSymbols ?? throw new ArgumentNullException("outputSymbols");
            this.Assignments = assignments ?? throw new ArgumentNullException("assignments");
            this.CurrentConstraint = currentConstraint ?? throw new ArgumentNullException("currentConstraint");
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
