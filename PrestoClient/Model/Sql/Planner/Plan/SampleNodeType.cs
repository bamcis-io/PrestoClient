using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.SampleNode.java (internal enum Type)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SampleNodeType
    {
        BERNOULLI,

        SYSTEM
    }
}
