using BAMCIS.PrestoClient.Model;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class TimeZoneKeyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TimeZoneKey Key = (TimeZoneKey)value;

            writer.WriteValue(Key.Key);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string Temp = reader.Value.ToString();

            short Value = short.Parse(Temp);

            return TimeZoneKey.GetTimeZoneKey(Value);
        }
    }
}
