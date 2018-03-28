using BAMCIS.PrestoClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BAMCIS.PrestoClient.Serialization
{
    public class DataSizeConverter : JsonConverter
    {
        private static readonly Regex Pattern = new Regex("([0-9]+(?:\\.[0-9]+)?)([a-zA-Z]+)?");

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(String));
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
            Match RegexMatch = Pattern.Match(Temp);

            if (RegexMatch.Success)
            {
                double Size = Double.Parse(RegexMatch.Groups[1].Value);
                string Unit = RegexMatch.Groups[2].Value;

                IEnumerable<DataSizeUnit> MatchingEnums = Enum.GetValues(typeof(DataSizeUnit)).Cast<DataSizeUnit>().Where(x => x.GetType().GetCustomAttribute<DescriptionAttribute>().Description == Unit);

                if (MatchingEnums.Any())
                {
                    return new DataSize(Size, MatchingEnums.First());
                }
                else
                {
                    throw new FormatException($"The unit type {Unit} for the value {Temp} is unknown and cannot be parsed.");
                }
            }
            else
            {
                throw new FormatException($"The value {Temp} is not formatted as a data size.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataSize Temp = (DataSize)value;

            writer.WriteRawValue(Temp.ToString());
        }
    }
}
