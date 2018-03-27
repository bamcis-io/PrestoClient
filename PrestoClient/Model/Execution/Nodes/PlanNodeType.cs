using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Execution.PlanFlattener
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
        TOPN
    }
}
