using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.ExchangeNode.java (internal enum Scope)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExchangeScope
    {
        LOCAL,
        REMOTE
    }
}
