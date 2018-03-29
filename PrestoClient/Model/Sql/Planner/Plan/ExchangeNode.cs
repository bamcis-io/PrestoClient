using BAMCIS.PrestoClient.Model.Sql.Planner;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{ 
    public class ExchangeNode : PlanNode
    {
        [JsonProperty(PropertyName = "type")]
        public string ExchangeType { get; set; }

        public string Scope { get; set; }

        public PartitioningScheme PartitioningScheme { get; set; }

        public IEnumerable<PlanNode> Sources { get; set; }

        public string[][] Inputs { get; set; }
    }
}
