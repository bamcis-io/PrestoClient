using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution.PlanFlattener
{
    public class OutputNode : PlanNode
    {
        public PlanNode Source { get; set; }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<string> Outputs { get; set; }
    }
}
