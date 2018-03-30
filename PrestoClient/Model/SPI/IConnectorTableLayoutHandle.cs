using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorTableLayoutHandle.java
    /// </summary>
    [JsonConverter(typeof(EmptyInterfaceConverter))]
    public interface IConnectorTableLayoutHandle
    {
        // Intentionally empty
    }
}
