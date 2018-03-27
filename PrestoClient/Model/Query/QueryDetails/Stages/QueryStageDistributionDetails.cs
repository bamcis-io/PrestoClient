using System;

namespace BAMCIS.PrestoClient.Model.Query.QueryDetails.Stages
{
    public class QueryStageDistributionDetails
    {
        public double MaxError { get; set; }
        public double Count { get; set; }
        public double Total { get; set; }
        public Int64 P01 { get; set; }
        public Int64 P05 { get; set; }
        public Int64 P10 { get; set; }
        public Int64 P25 { get; set; }
        public Int64 P50 { get; set; }
        public Int64 P75 { get; set; }
        public Int64 P90 { get; set; }
        public Int64 P95 { get; set; }
        public Int64 P99 { get; set; }
        public Int64 Min { get; set; }
        public Int64 Max { get; set; }
    }
}
