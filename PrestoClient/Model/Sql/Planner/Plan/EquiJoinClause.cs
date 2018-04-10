using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.JoinNode.java (internal class EquiJoinClause)
    /// </summary>
    public class EquiJoinClause
    {
        #region Public Properties

        public Symbol Left { get; }

        public Symbol Right { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public EquiJoinClause(Symbol left, Symbol right)
        {
            this.Left = left ?? throw new ArgumentNullException("left");
            this.Right = right ?? throw new ArgumentNullException("right");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.Left.ToString()} = {this.Right.ToString()}";
        }

        #endregion
    }
}
