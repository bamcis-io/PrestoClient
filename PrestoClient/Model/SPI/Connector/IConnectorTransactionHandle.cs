using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI.Connector
{
    /// <summary>
    /// From com.facebook.presto.spi.connector.ConnectorTransactionHandle.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorTransactionHandle
    {
        //Intentionally empty
    }
}
