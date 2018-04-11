using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PartitioningScheme.java
    /// </summary>
    public class PartitioningScheme
    {
        #region Public Properties

        public Partitioning Partitioning { get; }

        public IEnumerable<Symbol> OutputLayout { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashColumn { get; }

        public bool ReplicateNullsAndAny { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IEnumerable<int> BucketToPartition { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PartitioningScheme(
            Partitioning partitioning,
            IEnumerable<Symbol> outputLayout,
            Symbol hashColumn,
            bool replicateNullsAndAny,
            int[] bucketToPartition
            )
        {
            this.Partitioning = partitioning ?? throw new ArgumentNullException("partitioning");
            this.OutputLayout = outputLayout ?? throw new ArgumentNullException("outputLayout");

            HashSet<Symbol> Columns = this.Partitioning.GetColumns();

            ParameterCheck.Check(Columns.All(x => this.OutputLayout.Contains(x)), $"Output layout ({this.OutputLayout}) doesn't include all partition colums ({Columns}).");

            this.HashColumn = hashColumn;

            ParameterCheck.Check(!replicateNullsAndAny || Columns.Count <= 1, "Must have at most one partitioning column when nullPartition is REPLICATE.");
            this.ReplicateNullsAndAny = replicateNullsAndAny;
            this.BucketToPartition = bucketToPartition;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            PartitioningScheme That = (PartitioningScheme)obj;

            return Object.Equals(this.Partitioning, That.Partitioning) &&
                    Object.Equals(this.OutputLayout, That.OutputLayout) &&
                    this.ReplicateNullsAndAny == That.ReplicateNullsAndAny &&
                    Object.Equals(this.BucketToPartition, That.BucketToPartition);
        }

        public override int GetHashCode()
        {
            return this.Partitioning.GetHashCode() + this.OutputLayout.GetHashCode() + this.ReplicateNullsAndAny.GetHashCode() + this.BucketToPartition.GetHashCode();
        }

        public override string ToString()
        {

            return StringHelper.Build(this)
                    .Add("partitioning", this.Partitioning)
                    .Add("outputLayout", this.OutputLayout)
                    .Add("hashChannel", this.HashColumn)
                    .Add("replicateNullsAndAny", this.ReplicateNullsAndAny)
                    .Add("bucketToPartition", this.BucketToPartition)
                    .ToString();
        }

        #endregion

    }
}