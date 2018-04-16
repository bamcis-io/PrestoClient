using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Specification)
    /// </summary>
    public class Specification
    {
        #region Public Properties

        public IEnumerable<Symbol> PartitionBy { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public OrderingScheme OrderingScheme { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Specification(IEnumerable<Symbol> partitionBy, OrderingScheme orderingScheme)
        {
            this.PartitionBy = partitionBy ?? throw new ArgumentNullException("partitionBy");
            this.OrderingScheme = orderingScheme;
        }

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return Hashing.Hash(this.OrderingScheme, this.PartitionBy);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Specification) != obj.GetType())
            {
                return false;
            }

            Specification other = (Specification)obj;
           
            return this.PartitionBy.Equals(other.PartitionBy) &&
                     Object.Equals(this.OrderingScheme, other.OrderingScheme);
        }

        #endregion
    }
}
