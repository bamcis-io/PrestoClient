using BAMCIS.PrestoClient.Model.SPI.Block;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class TopNNode : PlanNode
    {
        public PlanNode Source { get; set; }

        public long Count { get; set; }

        public IEnumerable<Symbol> OrderBy { get; set; }

        /// <summary>
        /// TODO: Should be <Symbol, SortOrder> Problem with Json.NET
        /// </summary>
        public IDictionary<string, SortOrder> Orderings { get; set; }

        //public OrderingScheme OrderingScheme { get; set; }

        public Step Step { get; set; }
    }
}
