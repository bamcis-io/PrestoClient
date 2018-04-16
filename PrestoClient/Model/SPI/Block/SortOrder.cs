using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.SortOrder.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        ASC_NULLS_FIRST,
        ASC_NULLS_LAST,
        DESC_NULLS_FIRST,
        DESC_NULS_LAST
    }
}
