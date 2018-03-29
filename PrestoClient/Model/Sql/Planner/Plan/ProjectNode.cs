using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class ProjectNode : PlanNode
    {
        public PlanNode Source { get; set; }
        public IDictionary<string, IDictionary<string, string>> Assignments { get; set; }
    }
}
