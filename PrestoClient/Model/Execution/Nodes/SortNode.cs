namespace BAMCIS.PrestoClient.Model.Execution.PlanFlattener
{
    public class SortNode : PlanNode
    {
        public PlanNode Source { get; set; }
        public OrderingScheme OrderingScheme { get; set; }
    }
}
