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
        /// <summary>
        /// Add a default "NONE" so that when
        /// the classes containing this are populated
        /// from deserialization, it doesn't show "USER_ERROR"
        /// as the default since enums aren't nullable
        /// </summary>
        NONE,
        USER_ERROR,
        INTERNAL_ERROR,
        INSUFFICIENT_RESOURCES,
        EXTERNAL
    }
}
