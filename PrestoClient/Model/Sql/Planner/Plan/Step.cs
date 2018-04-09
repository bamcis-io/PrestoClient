using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.AggregationNode.java (internal enum Step)
    /// And com.facebook.presto.sql.planner.plan.TopNNode.java (internal enum Step)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Step
    {
        SINGLE,
        PARTIAL,
        FINAL,
        INTERMEDIATE
    }
}
