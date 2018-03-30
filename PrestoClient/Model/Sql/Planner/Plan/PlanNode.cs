using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    [JsonConverter(typeof(PlanNodeConverter))]
    public abstract class PlanNode
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
                { typeof(LimitNode), PlanNodeType.LIMIT },
                { typeof(AggregationNode), PlanNodeType.AGGREGATION },
                { typeof(ApplyNode), PlanNodeType.APPLY },
                { typeof(AssignUniqueId), PlanNodeType.ASSIGN_UNIQUE_ID },
                { typeof(DeleteNode), PlanNodeType.DELETE }
            };

            DerivedTypeToType = TypeToDerivedType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public PlanNode(PlanNodeId id)
        {
            this.Id = id ?? throw new ArgumentNullException("id");
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "@type")]
        public PlanNodeType NodeType
        {
            get
            {
                // This will return the actual type of the class
                // from derived classes
                return TypeToDerivedType[this.GetType()];
            }
        }

        public PlanNodeId Id { get; set; }

        public static Type GetType(PlanNodeType type)
        {
            return DerivedTypeToType[type];
        }


        // These will be made abstract after all existing node classes are updated
        public virtual IEnumerable<Symbol> GetOutputSymbols()
        {
            return null;
        }

        public virtual IEnumerable<PlanNode> GetSources()
        {
            return null;
        }
    }
}
