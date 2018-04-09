using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java
    /// </summary>
    public class WindowNode : PlanNode
    {
        #region Public Properties

        public PlanNode Source { get; }

        public Specification Specification { get; }

        /// <summary>
        /// TODO: Key should be Symbol
        /// </summary>
        public IDictionary<string, Function> WindowFunctions { get; }
       
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashSymbol { get; }

        public HashSet<Symbol> PrePartitionedInputs { get; }

        public int PreSortedOrderPrefix { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public WindowNode(
            PlanNodeId id, 
            PlanNode source, 
            Specification specification,
            IDictionary<string, Function> windowFunctions, 
            Symbol hashSymbol, 
            HashSet<Symbol> prePartitionedInputs, 
            int preSortedOrderPrefix
            ) : base(id)
        {
            this.Source = source ?? throw new ArgumentNullException("source");
            this.PrePartitionedInputs = prePartitionedInputs;
            this.Specification = specification ?? throw new ArgumentNullException("specification");
            this.PreSortedOrderPrefix = preSortedOrderPrefix;

            ParameterCheck.Check(this.PrePartitionedInputs.All(x => this.Specification.PartitionBy.Contains(x)), "Prepartitioned inputs must be contained in partitionBy.");


            ParameterCheck.Check(preSortedOrderPrefix == 0 || 
                (this.Specification.OrderingScheme != null && this.PreSortedOrderPrefix <= this.Specification.OrderingScheme.OrderBy.Count()), "Cannot have sorted more symbols than those requested.");
            ParameterCheck.Check(preSortedOrderPrefix == 0 || this.PrePartitionedInputs.Equals(this.Specification.PartitionBy), 
                "Presorted order prefix can only be greater than zero if all partition symbols are pre-partitioned");

            this.WindowFunctions = windowFunctions ?? throw new ArgumentNullException("windowFunctions");
            this.HashSymbol = hashSymbol;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.Source.GetOutputSymbols().Concat(this.WindowFunctions.Keys.Select(x => new Symbol(x)));
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Source;
        }

        #endregion
    }
}
