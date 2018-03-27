using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.PartitioningScheme.java
    /// </summary>
    public class PartitioningScheme
    {
        #region Public Properties

        public Partitioning Partitioning { get; set; }

        public IEnumerable<Symbol> OutputLayout { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol HashColumn { get; set; }

        public bool ReplicateNullsAndAny { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IEnumerable<int> BucketToPartition { get; set; }

        #endregion
    }
}