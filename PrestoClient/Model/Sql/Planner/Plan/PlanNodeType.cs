using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanNodeType
    {
        AGGREGATION,
        APPLY,
        ASSIGN_UNIQUE_ID,
        DELETE,
        EXCHANGE,
        BASE,
        REMOTESOURCE,
        SORT,
        OUTPUT,
        TABLESCAN,
        FILTER,
        PROJECT,
        TOPN,
        LIMIT
    }
}
