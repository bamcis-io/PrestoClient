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
            JObject Token = JObject.Load(reader);
            JObject Column = Token.GetValue("column", StringComparison.OrdinalIgnoreCase) as JObject;
            string ColumnType = Column.GetValue("@type", StringComparison.OrdinalIgnoreCase).ToObject<string>(serializer);

            JObject Domain = Token.GetValue("domain", StringComparison.OrdinalIgnoreCase) as JObject;
            bool NullAllowed = Domain.GetValue("nullAllowed", StringComparison.OrdinalIgnoreCase).ToObject<bool>(serializer);
            string DomainType = (Domain.GetValue("values", StringComparison.OrdinalIgnoreCase) as JObject).GetValue("@type", StringComparison.OrdinalIgnoreCase).ToObject<string>(serializer);
            string ItemType = (Domain.GetValue("values", StringComparison.OrdinalIgnoreCase) as JObject).GetValue("type", StringComparison.OrdinalIgnoreCase).ToObject<string>(serializer);

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
