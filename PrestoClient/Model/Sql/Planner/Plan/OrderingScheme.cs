using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class OrderingScheme
    {
        public IEnumerable<string> OrderBy { get; set; }

        public IDictionary<string, string> Orderings { get; set; }
    }
}
