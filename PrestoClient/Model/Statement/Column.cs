using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class Column
    {
        public string Name { get; set; }

        public string Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TypeSignature TypeSignature { get; set; }
    }
}