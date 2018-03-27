using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators
{
    /// <summary>
    /// This class is not currently used because the Operator Summary Info property
    /// is very dynamic when serialized as json by presto
    /// </summary>
    [JsonConverter(typeof(OperatorSummaryInfoConverter))]
    public class OperatorSummaryInfo
    {
        private static readonly Dictionary<Type, OperatorType> TypeToDerivedType;
        private static readonly Dictionary<OperatorType, Type> DerivedTypeToType;

        static OperatorSummaryInfo()
        {
            // These are the two types of known operator info
            TypeToDerivedType = new Dictionary<Type, OperatorType>()
            {
                { typeof(OperatorSummaryInfo), OperatorType.BASE },
                { typeof(SplitOperator), OperatorType.SPLITOPERATOR }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "@type")]
        public OperatorType Type
        {
            get
            {
                // This will return the actual type of the class
                // from derived classes
                return TypeToDerivedType[this.GetType()];
            }
        }

        public static Type GetType(OperatorType type)
        {
            return DerivedTypeToType[type];
        }
    }
}
