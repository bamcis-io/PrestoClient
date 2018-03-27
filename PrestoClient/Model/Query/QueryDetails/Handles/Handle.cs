using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles
{
    public class Handle
    {
        private static readonly Dictionary<Type, HandleType> TypeToDerivedType;
        private static readonly Dictionary<HandleType, Type> DerivedTypeToType;

        static Handle()
        {
            TypeToDerivedType = new Dictionary<Type, HandleType>()
            {
                { typeof(Handle), HandleType.BASE },
                { typeof(InfoSchemaHandle), HandleType.INFO_SCHEMA }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "@type")]
        public HandleType Type
        {
            get
            {
                // This will return the actual type of the class
                // from derived classes
                return TypeToDerivedType[this.GetType()];
            }
        }

        public static Type GetType(HandleType type)
        {
            return DerivedTypeToType[type];
        }
    }
}