using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace BAMCIS.PrestoClient.Serialization
{
    /// <summary>
    /// Provides a mechanism to convert a presto value from the JSON data that is returned into its corresponding type. The
    /// type information is usually provided in the Columns field and the value is found in the array of values contained 
    /// in the Data field.
    /// </summary>
    public static class PrestoTypeMapping
    {
        /// <summary>
        /// The available presto types and the conversion function for that type
        /// </summary>
        public static IDictionary<string, MappingConversionInfo> Types = new Dictionary<string, MappingConversionInfo>()
        {
           // Boolean
            { "boolean",        new MappingConversionInfo(typeof(bool),     input => Boolean.Parse(input)) },

            // Integer
            { "tinyint",        new MappingConversionInfo(typeof(sbyte),    input => SByte.Parse(input)) },
            { "smallint",       new MappingConversionInfo(typeof(short),    input => Int16.Parse(input)) },
            { "integer",        new MappingConversionInfo(typeof(int),      input => Int32.Parse(input)) },
            { "int",            new MappingConversionInfo(typeof(int),      input => Int32.Parse(input)) },
            { "bigint",         new MappingConversionInfo(typeof(long),     input => Int64.Parse(input)) },
            
            // Floating point
            { "real",           new MappingConversionInfo(typeof(float),    input => Single.Parse(input)) },
            { "double",         new MappingConversionInfo(typeof(double),   input => Double.Parse(input)) },

            // Fixed-Precision
            { "decimal",        new MappingConversionInfo(typeof(decimal),  input => Decimal.Parse(input)) },

            // String
            { "varchar",        new MappingConversionInfo(typeof(string),   input => input) },
            { "char",           new MappingConversionInfo(typeof(char[]),   input => {
                if (input.Length == 1)
                {
                    return new char[] { Char.Parse(input) };
                }
                else
                {
                    return input.ToCharArray();
                }
            })},
            { "varbinary",      new MappingConversionInfo(typeof(byte[]),   input => {
                    if (input.Length % 2 != 0)
                    {
                        throw new ArgumentException($"The varbinary string {input} cannot have an odd number of digits.");
                    }

                    if (input.StartsWith("0x"))
                    {
                        input = input.Substring(2);
                    }

                    // We want to remove the leading 0x prefix
                    byte[] HexAsBytes = new byte[input.Length / 2];
                    for (int index = 0; index < HexAsBytes.Length; index++)
                    {
                        string byteValue = input.Substring(index * 2, 2);
                        HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }

                    return HexAsBytes;
                })},
            { "json",           new MappingConversionInfo(typeof(string),   input => input) },

            // Structural
            {"array",           new MappingConversionInfo(typeof(Array),    input => input) },
            {"map",             new MappingConversionInfo(typeof(object),   input => input) },
            {"row",             new MappingConversionInfo(typeof(IDictionary<string, object>),     input => input) },

            // Network Address
            {"ipaddress",       new MappingConversionInfo(typeof(IPAddress),    input => IPAddress.Parse(input)) }
        };

        /// <summary>
        /// Converts a string input with the provided presto type into
        /// the corresponding .NET object. For example, a bigint is converted to a long,
        /// varchar to string, and ipaddress to IPAddress. The prestoType parameter
        /// should be specified from the TypeSignature.RawType property from the column information,
        /// so used "array" vs "array(varchar)".
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="prestoType">The declared raw presto type</param>
        /// <returns></returns>
        public static object Convert(string value, string prestoType)
        {
            if (PrestoTypeMapping.Types.ContainsKey(prestoType.ToLower()))
            {
                return Types[prestoType.ToLower()].Converter.Invoke(value);
            }
            else
            {
                throw new KeyNotFoundException($"The presto type {prestoType} could not be found.");
            }
        }
    }
}