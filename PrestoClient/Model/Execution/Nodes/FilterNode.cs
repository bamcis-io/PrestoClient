namespace BAMCIS.PrestoClient.Model.Execution.PlanFlattener
{
    public class FilterNode : PlanNode
    {
        public PlanNode Source { get; set; }
        public string Predicate { get; set; }
    }
}
