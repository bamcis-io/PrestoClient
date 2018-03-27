namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Operators
{
    /// <summary>
    /// This class is not currently used because the Operator Summary Info property
    /// is very dynamic when serialized as json by presto
    /// </summary>
    public class SplitOperator : OperatorSummaryInfo
    {
        public SplitOperatorInfo SplitInfo { get; set; }
    }
}
