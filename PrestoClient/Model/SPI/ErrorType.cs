using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ErrorType.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType
    {
        USER_ERROR,
        INTERNAL_ERROR,
        INSUFFICIENT_RESOURCES,
        EXTERNAL
    }
}
