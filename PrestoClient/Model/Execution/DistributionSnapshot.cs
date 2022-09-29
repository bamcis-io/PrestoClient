using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From io.airlift.stats.Distribution.java (internal class DistributionSnapshot)
    /// </summary>
    public class DistributionSnapshot
    {
        #region Public Properties

        public double MaxError { get; }

        public double Count { get; }

        public double Total { get; }

        public double P01 { get; }

        public double P05 { get; }

        public double P10 { get; }

        public double P25 { get; }

        public double P50 { get; }

        public double P75 { get; }

        public double P90 { get; }

        public double P95 { get; }

        public double P99 { get; }

        public double Min { get; }

        public double Max { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DistributionSnapshot(
            double maxError,
            double count,
            double total,
            double p01,
            double p05,
            double p10,
            double p25,
            double p50,
            double p75,
            double p90,
            double p95,
            double p99,
            double min,
            double max
            )
        {
            this.MaxError = maxError;
            this.Count = count;
            this.Total = total;
            this.P01 = p01;
            this.P05 = p05;
            this.P10 = p10;
            this.P25 = p25;
            this.P50 = p50;
            this.P75 = p75;
            this.P90 = p90;
            this.P95 = p95;
            this.P99 = p99;
            this.Min = min;
            this.Max = max;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxError", this.MaxError)
                .Add("count", this.Count)
                .Add("total", this.Total)
                .Add("p01", this.P01)
                .Add("p05", this.P05)
                .Add("p10", this.P10)
                .Add("p25", this.P25)
                .Add("p50", this.P50)
                .Add("p75", this.P75)
                .Add("p90", this.P90)
                .Add("p95", this.P95)
                .Add("p99", this.P99)
                .Add("min", this.Min)
                .Add("mix", this.Max)
                .ToString();
        }

        #endregion
    }
}
