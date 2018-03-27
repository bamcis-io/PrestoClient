using System;

namespace BAMCIS.PrestoClient.Serialization
{
    public class MappingConversionInfo
    {
        public delegate object PrestoTypeConverter(string value);

        public PrestoTypeConverter Converter { get; set; }

        public Type DotNetType { get; set; }

        public MappingConversionInfo(Type type, PrestoTypeConverter converter)
        {
            this.Converter = converter ?? throw new ArgumentNullException("The converter cannot be null.");
            this.DotNetType = type;
        }
    }
}