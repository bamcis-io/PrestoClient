using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.SPI.Block
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        ASC_NULLS_FIRST,
        ASC_NULLS_LAST,
        DESC_NULLS_FIRST,
        DESC_NULS_LAST
    }
}
