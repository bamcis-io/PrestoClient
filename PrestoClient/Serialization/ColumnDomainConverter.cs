using BAMCIS.PrestoClient.Model.SPI.Predicate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class ColumnDomainConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ColumnDomain<>);
        }

        public override bool CanRead => false;

        public override bool CanWrite => false;

        /// <summary>
        /// TODO: This needs to be implemented once the different types of columns and domains are known
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {               
            JToken Token = JToken.Load(reader);
            JToken Column = Token["column"];
            string ColumnType = Column["@type"].ToObject<string>();

            JToken Domain = Token["domain"];
            bool NullAllowed = Domain["nullAllowed"].ToObject<bool>();
            string DomainType = Domain["values"]["@type"].ToObject<string>();
            string ItemType = Domain["values"]["type"].ToObject<string>();

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
