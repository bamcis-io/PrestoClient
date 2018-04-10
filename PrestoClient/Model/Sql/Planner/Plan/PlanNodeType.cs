using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanNodeType
    {
        BASE,

        AGGREGATION,
        APPLY,
        ASSIGNUNIQUEID,
        DELETE,
        DISTINCTLIMIT,
        EXCHANGE,
        EXPLAINANALYZE,
        FILTER,
        GROUPID,
        INDEXJOIN,
        INDEXSOURCE,
        INTERSECT,
        JOIN,
        LATERALJOIN,
        LIMIT,
        MARKDISTINCT,
        METADATADELETE,
        OUTPUT,
        PROJECT,
        REMOTESOURCE,
        ROWNUMBER,
        SAMPLE,
        SCALAR,
        SEMIJOIN,
        SORT,
        TABLECOMMIT,
        TABLESCAN,
        TABLEWRITER,
        TOPN,
        TOPNROWNUMBER,
        UNION,
        UNNEST,
        VALUES,
        WINDOW
    }
}
