using BAMCIS.PrestoClient.Model.Execution;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Model.Sql.Planner;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Serialization
{
    public class FlattenedPlanFragmentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FlattenedPlanFragment);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken Tokens = JToken.Load(reader);

            // These two are native to the flattened plan 
            string TextPlan = Tokens["textPlan"].ToString();
            IEnumerable<FlattenedNode> Nodes = Tokens["nodes"].ToObject<IEnumerable<FlattenedNode>>();

            // These properties are derived from the PlanFragment
            PlanFragmentId Id = new PlanFragmentId(Tokens["id"].ToObject<string>(serializer));
            PlanNode Root = Tokens["tree"].ToObject<PlanNode>(serializer);
            IDictionary<string, string> Symbols = Tokens["symbols"].ToObject<Dictionary<string, string>>(serializer);
            PartitioningHandle Partitioning = Tokens["partitioning"].ToObject<PartitioningHandle>(serializer);
            IEnumerable<PlanNodeId> PartitionedSources = Tokens["partitionedSources"].ToObject<IEnumerable<PlanNodeId>>(serializer);
            PartitioningScheme PartitioningScheme = Tokens["partitioningScheme"].ToObject<PartitioningScheme>(serializer);

            // The pipeline execution strategy doesn't really matter because the flattened plan doesn't expose it
            PlanFragment Fragment = new PlanFragment(Id, Root, Symbols, Partitioning, PartitionedSources, PartitioningScheme, PipelineExecutionStrategy.UNGROUPED_EXECUTION);

            return new FlattenedPlanFragment(TextPlan, Fragment, Nodes);
        }

        public override bool CanRead => true;

        /// <summary>
        ///  Use default writer
        /// </summary>
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
