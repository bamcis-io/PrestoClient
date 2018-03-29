using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class OutputNode : PlanNode
    {
        public PlanNode Source { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<string> Outputs { get; set; }
    }
}
