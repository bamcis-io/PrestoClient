using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.ParameterKind.java
    /// </summary>
    [JsonConverter(typeof(ParameterKindConverter))]
    public enum ParameterKind
    {
        [Description("TYPE_SIGNATURE")]
        TYPE,

        [Description("NAMED_TYPE_SIGNATURE")]
        NAMED_TYPE,

        [Description("LONG_LITERAL")]
        LONG,

        [Description("")]
        VARIABLE
    }
}
