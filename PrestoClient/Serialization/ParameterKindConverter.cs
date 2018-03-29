using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

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
            string Temp = reader.Value.ToString();

            if (String.IsNullOrEmpty(Temp))
            {
                return ParameterKind.VARIABLE;
            }
            else
            {
                IEnumerable<ParameterKind> MatchingEnums = Enum.GetValues(typeof(ParameterKind)).Cast<ParameterKind>().Where(x => x.GetType().GetMember(x.ToString()).FirstOrDefault().GetCustomAttribute<DescriptionAttribute>().Description == Temp);

                if (MatchingEnums.Any())
                {
                    return MatchingEnums.First();
                }
                else
                {
                    return ParameterKind.VARIABLE;
                }                
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ParameterKind Val = (ParameterKind)value;

            writer.WriteRawValue(Val.GetType().GetCustomAttribute<DescriptionAttribute>().Description);
        }
    }
}
