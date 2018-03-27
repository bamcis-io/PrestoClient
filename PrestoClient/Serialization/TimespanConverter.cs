using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace BAMCIS.PrestoClient.Serialization
{
    /// <summary>
    /// Converts the presto timespan string representation into a Timespan object and back. 
    /// Presto uses values like 3.14s, 2h, 1.001ms, 15.73us, etc. 
    /// </summary>
    public class TimespanConverter : JsonConverter
    {
        /*
         * Match items like
         *  3.14s
         *  0.00ns
         *  15.73us
         *  1d
         *  1s
         *  2h
         *  1.0ms
         */
        private Regex TimespanRegex = new Regex("^([0-9]+(?:\\.[0-9]+)?)([a-z]+)$");

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

            Match RegexMatch = TimespanRegex.Match(Temp);

            if (RegexMatch.Success)
            {
                decimal Time = Decimal.Parse(RegexMatch.Groups[1].Value);
                string Format = RegexMatch.Groups[2].Value;
                decimal Nano = 0;

                switch (Format)
                {
                    case "d":
                        {
                            Nano = Time * 24 * 60 * 60 * 1000 * 1000 * 1000;
                            break;
                        }
                    case "h":
                        {
                            Nano = Time * 60 * 60 * 1000 * 1000 * 1000;
                            break;
                        }
                    case "m":
                        {
                            Nano = Time * 60 * 1000 * 1000 * 1000;
                            break;
                        }
                    case "s":
                        {
                            Nano = Time * 1000 * 1000 * 1000;
                            break;
                        }
                    case "ms":
                        {
                            Nano = Time * 1000 * 1000;
                            break;
                        }
                    case "mu":
                    case "us":
                        {
                            Nano = Time * 1000;
                            break;
                        }
                    case "ns":
                        {
                            Nano = Time;
                            break;
                        }
                    default:
                        {
                            throw new FormatException($"The format {Format} for the time value {Temp} is unknown and cannot be parsed.");
                        }
                }

                // A tick is one hundred nanoseconds
                return new TimeSpan((long)Math.Truncate(Nano) / 100);
            }
            else
            {
                throw new FormatException($"The time value {Temp} is not formatted as a presto timespan string.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TimeSpan Temp = (TimeSpan)value;
            string Value = "";

            if (Temp.TotalDays > 1)
            {
                Value = $"{Temp.TotalDays.ToString("0.00")}d";
            }
            else if (Temp.TotalHours > 1)
            {
                Value = $"{Temp.TotalHours.ToString("0.00")}d";
            }
            else if (Temp.TotalMinutes > 1)
            {
                Value = $"{Temp.TotalMinutes.ToString("0.00")}d";
            }
            else if (Temp.TotalSeconds > 1)
            {
                Value = $"{Temp.TotalSeconds.ToString("0.00")}d";
            }
            else if (Temp.TotalMilliseconds > 1)
            {
                Value = $"{Temp.TotalMilliseconds.ToString("0.00")}d";
            }
            else
            {
                long Ticks = Temp.Ticks;

                // Microseconds
                if (Ticks / 1000 > 1)
                {
                    Value = $"{(Ticks / 1000).ToString("0.00")}us";
                }
                else // Nanoseconds
                {
                    Value = $"{(Ticks / 100).ToString("0.00")}ns";
                }
            }

            writer.WriteRawValue(Value);
        }
    }
}