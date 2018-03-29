using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanNodeType
    {
        BASE,
        REMOTESOURCE,
        SORT,
        EXCHANGE,
        OUTPUT,
        TABLESCAN,
        FILTER,
        PROJECT,
        TOPN,
        LIMIT
    }
}
