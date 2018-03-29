using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(PlanNodeConverter))]
    public class PlanNode
    {
        private static readonly Dictionary<Type, PlanNodeType> TypeToDerivedType;
        private static readonly Dictionary<PlanNodeType, Type> DerivedTypeToType;

        static PlanNode()
        {
            TypeToDerivedType = new Dictionary<Type, PlanNodeType>()
            {
                { typeof(PlanNode), PlanNodeType.BASE },
                { typeof(OutputNode), PlanNodeType.OUTPUT },
                { typeof(RemoteSourceNode), PlanNodeType.REMOTESOURCE },
                { typeof(TableScanNode), PlanNodeType.TABLESCAN },
                { typeof(ExchangeNode), PlanNodeType.EXCHANGE },
                { typeof(FilterNode), PlanNodeType.FILTER },
                { typeof(SortNode), PlanNodeType.SORT },
                { typeof(ProjectNode), PlanNodeType.PROJECT },
                { typeof(TopNNode), PlanNodeType.TOPN },
                { typeof(LimitNode), PlanNodeType.LIMIT }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "@type")]
        public PlanNodeType Type
        {
            get
            {
                // This will return the actual type of the class
                // from derived classes
                return TypeToDerivedType[this.GetType()];
            }
        }

        public string Id { get; set; }

        public static Type GetType(PlanNodeType type)
        {
            return DerivedTypeToType[type];
        }
    }
}
