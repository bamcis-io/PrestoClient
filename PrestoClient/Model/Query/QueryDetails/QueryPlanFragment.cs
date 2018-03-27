using Newtonsoft.Json;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Handles;
using BAMCIS.PrestoClient.Model.Query.QueryDetails.Nodes;
using BAMCIS.PrestoClient.Serialization;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails
{
    public class QueryPlanFragment
    {
        public string TextPlan { get; set; }

        public IEnumerable<QueryPlanNode> Nodes { get; set; }

        public IEnumerable<RemoteSourceNode> RemoteSourceNodes { get; set; }

        public IEnumerable<string> PartitionedSources { get; set; }

        public IEnumerable<QueryPlanNode> PartitionedSourceNodes { get; set; }

        public IEnumerable<string> Types { get; set; }

        public PartitioningScheme PartitioningScheme { get; set; }

        public ConnectorHandleWrapper Partitioning { get; set; }

        public IDictionary<string, string> Symbols { get; set; }

        public string Id { get; set; }

        public QueryPlanNode Tree { get; set; }
    }
}