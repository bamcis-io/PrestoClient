using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExchangeNode.java (internal enum Type)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExchangeType
    {
        GATHER,
        REPARTITION,
        REPLICATE
    }
}
