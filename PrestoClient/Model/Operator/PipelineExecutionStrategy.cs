using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.PipelineExecutionStrategy.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PipelineExecutionStrategy
    {
        UNGROUPED_EXECUTION,
        GROUPED_EXECUTION,
    }
}
