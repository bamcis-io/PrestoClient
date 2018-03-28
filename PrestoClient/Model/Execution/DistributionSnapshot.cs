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

        public long P01 { get; }

        public long P05 { get; }

        public long P10 { get; }

        public long P25 { get; }

        public long P50 { get; }

        public long P75 { get; }

        public long P90 { get; }

        public long P95 { get; }

        public long P99 { get; }

        public long Min { get; }

        public long Max { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DistributionSnapshot(
            double maxError,
            double count,
            double total,
            long p01,
            long p05,
            long p10,
            long p25,
            long p50,
            long p75,
            long p90,
            long p95,
            long p99,
            long min,
            long max
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
            return $"DistributionSnapshot {{maxError={this.MaxError}, count={this.Count}, total={this.Total}, p01={this.P01}, p05={this.P05}, p10={this.P10}, p25={this.P25}, p50={this.P50}, p75={this.P75}, p90={this.P90}, p95={this.P95}, p99={this.P99}, min={this.Min}, max={this.Max}}}";
        }

        #endregion
    }
}
