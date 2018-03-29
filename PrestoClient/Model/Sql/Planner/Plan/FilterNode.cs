namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    public class FilterNode : PlanNode
    {
        public PlanNode Source { get; set; }
        public string Predicate { get; set; }
    }
}
