namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class SortNode : PlanNode
    {
        public PlanNode Source { get; set; }
        public OrderingScheme OrderingScheme { get; set; }
    }
}
