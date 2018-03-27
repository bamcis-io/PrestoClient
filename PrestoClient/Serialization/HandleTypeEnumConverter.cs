using Newtonsoft.Json;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class HandleTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HandleType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string Temp = reader.Value.ToString().TrimStart('$');

            return Enum.Parse(typeof(HandleType), Temp);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string Temp = $"${value.ToString().ToLower()}";

            writer.WriteValue(Temp);
        }
    }
}