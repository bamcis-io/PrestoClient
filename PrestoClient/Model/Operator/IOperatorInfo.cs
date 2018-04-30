using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Operator
{
    /// <summary>
    /// From com.facebook.presto.operator.OperatorInfo.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IOperatorInfo
    {
        bool IsFinal();
    }
}
