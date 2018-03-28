using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From io.airlift.units.DataSize.java (internal class DataSizeUnit)
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataSizeUnit
    {
        [Description("B")]
        BYTE,

        [Description("kB")]
        KILOBYTE,

        [Description("MB")]
        MEGABYTE,

        [Description("GB")]
        GIGABYTE,

        [Description("TB")]
        TERABYTE,

        [Description("PB")]
        PETABYTE,

        [Description("EB")]
        EXABYTE
    }
}
