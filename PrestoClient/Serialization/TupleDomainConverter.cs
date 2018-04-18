using BAMCIS.PrestoClient.Model.SPI.Predicate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Serialization
{
    public class TupleDomainConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TupleDomain<>);
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

        /// <summary>
        /// TODO: This needs to be implemented to account for the generic type
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject Token = JObject.Load(reader);
            IEnumerable<ColumnDomain<T>> Domains = Token.GetValue("columnDomains", StringComparison.OrdinalIgnoreCase).ToObject<IEnumerable<ColumnDomain<T>>>();

            return TupleDomain<T>.FromColumnDomains(Domains);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
