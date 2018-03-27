using Newtonsoft.Json;
using BAMCIS.PrestoClient.Serialization;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles
{
    [JsonConverter(typeof(HandleTypeEnumConverter))]
    public enum HandleType
    {
        BASE,
        INFO_SCHEMA,
        REMOTE
    }
}