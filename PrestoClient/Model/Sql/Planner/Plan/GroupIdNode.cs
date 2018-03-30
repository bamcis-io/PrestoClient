using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.GroupIdNode.java
    /// </summary>
    public class GroupIdNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public IEnumerable<List<Symbol>> GroupingSets { get; }

        /// <summary>
        /// TODO: Key is supposed to be Symbol
        /// </summary>
        public IDictionary<string, Symbol> GroupingSetMappings { get; }


        /// <summary>
        /// TODO: Key is supposed to be Symbol
        /// </summary>
        public IDictionary<string, Symbol> ArgumentMappings { get; }

        public Symbol GroupIdSymbol { get; }

        #endregion

        #region Constructors

        public GroupIdNode(
            PlanNodeId id, 
            PlanNode source, 
            IEnumerable<List<Symbol>> groupingSets, 
            IDictionary<string, Symbol> groupingSetMappings, 
            IDictionary<string, Symbol> argumentMappings, 
            Symbol groupIdSymbol
            ) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.GroupingSets = groupingSets ?? throw new ArgumentNullException("groupingSets");
            this.GroupingSetMappings = groupingSetMappings ?? throw new ArgumentNullException("groupingSetMappings");
            this.ArgumentMappings = argumentMappings ?? throw new ArgumentNullException("argumentMappings");
            this.GroupIdSymbol = groupIdSymbol ?? throw new ArgumentNullException("groupIdSymbol");

            if (this.GroupingSetMappings.Keys.Intersect(this.ArgumentMappings.Keys).Any())
            {
                throw new ArgumentException("The argument outputs and grouping outputs must be a disjoint set.");
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.GroupingSets.SelectMany(x => x).Concat(this.ArgumentMappings.Keys.Select(x => new Symbol(x))).Concat(new Symbol[] { this.GroupIdSymbol });
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
