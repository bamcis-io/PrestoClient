using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class ToStringJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                string Temp = reader.Value.ToString();

                return Activator.CreateInstance(objectType, new string[] { Temp });
            }
            else
            {
                return null;
            }
        }
    }
}
