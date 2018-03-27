using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Server
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ThreadState
    {
        TIMED_WAITING,
        WAITING,
        RUNNABLE,
        FAILED
    }
}