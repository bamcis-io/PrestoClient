namespace BAMCIS.PrestoClient.Model.Execution.PlanFlattener
{
    public class TopNNode : PlanNode
    {
        public PlanNode Source { get; set; }

        public int Count { get; set; }

        public OrderingScheme OrderingScheme { get; set; }

        /// <summary>
        /// TODO: This should be an enum
        /// </summary>
        public string Step { get; set; }
    }
}
