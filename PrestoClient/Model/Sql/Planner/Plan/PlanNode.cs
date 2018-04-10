using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BAMCIS.PrestoClient.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    //[JsonConverter(typeof(PlanNodeConverter))]
    public abstract class PlanNode
    {
        private static readonly Dictionary<Type, PlanNodeType> TypeToDerivedType;
        private static readonly Dictionary<PlanNodeType, Type> DerivedTypeToType;

        static PlanNode()
        {
            TypeToDerivedType = new Dictionary<Type, PlanNodeType>()
            {
                { typeof(PlanNode), PlanNodeType.BASE },

                { typeof(AggregationNode), PlanNodeType.AGGREGATION },
                { typeof(ApplyNode), PlanNodeType.APPLY },
                { typeof(AssignUniqueId), PlanNodeType.ASSIGNUNIQUEID },

                { typeof(DeleteNode), PlanNodeType.DELETE },
                { typeof(DistinctLimitNode), PlanNodeType.DISTINCTLIMIT },

                { typeof(ExchangeNode), PlanNodeType.EXCHANGE },
                { typeof(ExplainAnalyzeNode), PlanNodeType.EXPLAINANALYZE },

                { typeof(FilterNode), PlanNodeType.FILTER },

                { typeof(GroupIdNode), PlanNodeType.GROUPID },

                { typeof(IndexJoinNode), PlanNodeType.INDEXJOIN },
                { typeof(IndexSourceNode), PlanNodeType.INDEXSOURCE },

                { typeof(JoinNode), PlanNodeType.JOIN },

                { typeof(LateralJoinNode), PlanNodeType.LATERALJOIN },
                { typeof(LimitNode), PlanNodeType.LIMIT },

                { typeof(MarkDistinctNode), PlanNodeType.MARKDISTINCT },
                { typeof(MetadataDeleteNode), PlanNodeType.METADATADELETE },

                { typeof(OutputNode), PlanNodeType.OUTPUT },

                { typeof(ProjectNode), PlanNodeType.PROJECT },

                { typeof(RemoteSourceNode), PlanNodeType.REMOTESOURCE },
                { typeof(RowNumberNode), PlanNodeType.ROWNUMBER },

                { typeof(SampleNode), PlanNodeType.SAMPLE },
                { typeof(EnforceSingleRowNode), PlanNodeType.SCALAR },
                { typeof(SemiJoinNode), PlanNodeType.SEMIJOIN },
                { typeof(SortNode), PlanNodeType.SORT },

                { typeof(TableFinishNode), PlanNodeType.TABLECOMMIT },
                { typeof(TableScanNode), PlanNodeType.TABLESCAN },
                { typeof(TableWriterNode), PlanNodeType.TABLEWRITER },
                { typeof(TopNNode), PlanNodeType.TOPN },

                { typeof(TopNRowNumberNode), PlanNodeType.TOPNROWNUMBER },

                { typeof(UnionNode), PlanNodeType.UNION },
                { typeof(UnnestNode), PlanNodeType.UNNEST },

                { typeof(ValuesNode), PlanNodeType.VALUES },

                { typeof(WindowNode), PlanNodeType.WINDOW }
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

        public abstract IEnumerable<Symbol> GetOutputSymbols();

        public abstract IEnumerable<PlanNode> GetSources();
    }
}
