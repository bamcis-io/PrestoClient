using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class RemoteSourceNode : PlanNode
    {
        public IEnumerable<string> SourceFragmentIds { get; set; }
        public IEnumerable<string> Outputs { get; set; }
    }
}
