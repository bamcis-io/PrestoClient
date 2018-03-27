using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators
{
    /// <summary>
    /// This class is not currently used because the Operator Summary Info property
    /// is very dynamic when serialized as json by presto
    /// </summary>
    // [JsonConverter(typeof(PrestoSplitInfoConverter))]
    public class SplitOperatorInfo
    {
        private static readonly Dictionary<Type, SplitOperatorInfoType> TypeToDerivedType;
        private static readonly Dictionary<SplitOperatorInfoType, Type> DerivedTypeToType;

        static SplitOperatorInfo()
        {
            TypeToDerivedType = new Dictionary<Type, SplitOperatorInfoType>()
            {
                { typeof(SplitOperatorInfo), SplitOperatorInfoType.BASE },
                { typeof(SplitOperatorSchemaInfo), SplitOperatorInfoType.INFO_SCHEMA }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "@type")]
        public SplitOperatorInfoType Type
        {
            get
            {
                // This will return the actual type of the class
                // from derived classes
                return TypeToDerivedType[this.GetType()];
            }
        }

        public static Type GetType(SplitOperatorInfoType type)
        {
            return DerivedTypeToType[type];
        }
    }
}
