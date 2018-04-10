using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.JoinNode.java
    /// </summary>
    public class JoinNode : PlanNode
    {
        #region Public Properties

        public JoinType Type { get; }

        public PlanNode Left { get; }

        public PlanNode Right { get; }

        public IEnumerable<EquiJoinClause> Criteria { get; }

        /**
         * List of output symbols produced by join. Output symbols
         * must be from either left or right side of join. Symbols
         * from left join side must precede symbols from right side
         * of join.
         */
        public IEnumerable<Symbol> OutputSymbols { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public dynamic Filter { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol LeftHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol RightHashSymbol { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public DistributionType DistributionType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public bool SpatialJoin { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public JoinNode(
            PlanNodeId id, 
            JoinType type,
            PlanNode left,
            PlanNode right,
            IEnumerable<EquiJoinClause> criteria,
            IEnumerable<Symbol> outputSymbols,
            dynamic filter,
            Symbol leftHashSymbol,
            Symbol rightHashSymbol,
            DistributionType distributionType
            ) : base(id)
        {
            this.Type = type;
            this.Left = left ?? throw new ArgumentNullException("left");
            this.Right = right ?? throw new ArgumentNullException("right");
            this.Criteria = criteria ?? throw new ArgumentNullException("criteria");
            this.OutputSymbols = outputSymbols ?? throw new ArgumentNullException("outputSymbols");
            this.Filter = filter;
            this.LeftHashSymbol = leftHashSymbol;
            this.RightHashSymbol = rightHashSymbol;
            this.DistributionType = distributionType;

            HashSet<Symbol> InputSymbols = new HashSet<Symbol>(this.Left.GetOutputSymbols().Concat(this.Right.GetOutputSymbols()));

            ParameterCheck.Check(this.OutputSymbols.All(x => InputSymbols.Contains(x)), "Left and right join inputs do not contain all output symbols.");

            ParameterCheck.Check(!this.IsCrossJoin() || InputSymbols.Equals(this.OutputSymbols), "Cross join does not support output symbols pruning or reordering.");

            ParameterCheck.Check(!(!this.Criteria.Any() && this.LeftHashSymbol != null), "Left hash symbol is only valid in equijoin.");
            ParameterCheck.Check(!(!this.Criteria.Any() && this.RightHashSymbol != null), "Right hash symbol is only valid in equijoin.");
        }

        #endregion

        #region Public Methods

        public bool IsCrossJoin()
        {
            // Criteria is empty and no filter and join type is inner then it is a cross join
            return !this.Criteria.Any() && this.Filter == null && this.Type == JoinType.INNER;
        }

        public override IEnumerable<Symbol> GetOutputSymbols()
        {
            return this.OutputSymbols;
        }

        public override IEnumerable<PlanNode> GetSources()
        {
            yield return this.Left;

            yield return this.Right;
        }

        #endregion
    }
}
