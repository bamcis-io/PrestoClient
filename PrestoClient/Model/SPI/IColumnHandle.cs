using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ColumnHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IColumnHandle
    {
        // Intentionally empty
    }
}
