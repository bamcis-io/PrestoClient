using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection;

namespace BAMCIS.PrestoClient.Serialization
{
    public class ParameterKindConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ParameterKind));
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string Temp = reader.ReadAsString();

            if (String.IsNullOrEmpty(Temp))
            {
                return ParameterKind.VARIABLE;
            }
            else
            {
                foreach (ParameterKind Item in Enum.GetValues(typeof(ParameterKind)))
                {
                    string Desc = Item.GetType().GetCustomAttribute<DescriptionAttribute>().Description;

                    if (Temp == Desc)
                    {
                        return Item;
                    }
                }

                return ParameterKind.VARIABLE;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ParameterKind Val = (ParameterKind)value;

            writer.WriteValue(Val.GetType().GetCustomAttribute<DescriptionAttribute>().Description);
        }
    }
}
