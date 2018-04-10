using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SemiJoinNode.java (internal enum DistributionType)
    /// also com.facebook.presto.sql.planner.plan.JoinNode.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DistributionType
    {
        PARTITIONED,
        REPLICATED
    }
}
