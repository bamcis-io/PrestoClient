using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// A counter that decays exponentially. Values are weighted according to the formula
    /// w(t, α) = e^(-α * t), where α is the decay factor and t is the age in seconds
    /// 
    /// The implementation is based on the ideas from
    /// http://www.research.att.com/people/Cormode_Graham/library/publications/CormodeShkapenyukSrivastavaXu09.pdf
    /// to not have to rely on a timer that decays the value periodically
    /// 
    /// From io.airlift.stats.DecayCounter.java
    /// </summary>
    public class DecayCounter
    {
        #region Private Fields

        // needs to be such that Math.exp(alpha * seconds) does not grow too big
        private static readonly long RESCALE_THRESHOLD_SECONDS = 50;

        private volatile object SyncRoot = new object();

        private double Count;

        private long LandmarkInSeconds;

        #endregion

        #region Public Properties

        public double Alpha { get; }
        
        #endregion

        #region Constructors

        public DecayCounter(double alpha)
        {
            this.Alpha = alpha;
            this.LandmarkInSeconds = this.GetTickInSeconds();
        }

        #endregion

        #region Public Methods

        public void Add(long value)
        {
            lock (SyncRoot)
            {
                long NowInSeconds = this.GetTickInSeconds();

                if (NowInSeconds - this.LandmarkInSeconds >= RESCALE_THRESHOLD_SECONDS)
                {
                    this.RescaleToNewLandmark(NowInSeconds);
                }

                this.Count += value * this.Weight(NowInSeconds, this.LandmarkInSeconds);
            }
        }

        public void Merge(DecayCounter decayCounter)
        {
            if (decayCounter == null)
            {
                throw new ArgumentNullException("decayCounter");
            }

            ParameterCheck.Check(decayCounter.Alpha == this.Alpha, $"Expected decayCounter to have alpha {this.Alpha}, but was {decayCounter.Alpha}.");

            lock(SyncRoot)
            {
                // if the landmark this counter is behind the other counter
                if (this.LandmarkInSeconds < decayCounter.LandmarkInSeconds)
                {
                    // rescale this counter to the other counter, and add
                    this.RescaleToNewLandmark(decayCounter.LandmarkInSeconds);
                    this.Count += decayCounter.Count;
                }
                else
                {
                    // rescale the other counter and add
                    double OtherRescaledCount = decayCounter.Count / this.Weight(this.LandmarkInSeconds, decayCounter.LandmarkInSeconds);
                    this.Count += OtherRescaledCount;
                }
            }
        }

        public void Reset()
        {
            lock (SyncRoot)
            {
                this.LandmarkInSeconds = GetTickInSeconds();
                this.Count = 0;
            }
        }

        public double GetCount()
        {
            lock (SyncRoot)
            {
                long NowInSeconds = GetTickInSeconds();
                return this.Count / this.Weight(NowInSeconds, this.LandmarkInSeconds);
            }
        }

        public double GetRate()
        {
            lock (SyncRoot)
            {
                return this.GetCount() * this.Alpha;
            }
        }

        public DecayCounterSnapshot Snapshot()
        {
            return new DecayCounterSnapshot(this.GetCount(), this.GetRate());
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("count", this.GetCount())
                .Add("rate", this.GetRate())
                .ToString();
        }

        #endregion

        #region Private Methods

        private long GetTickInSeconds()
        {
            return (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond);
        }

        private void RescaleToNewLandmark(long newLandMarkInSeconds)
        {
            // rescale the count based on a new landmark to avoid numerical overflow issues
            this.Count = this.Count / Weight(newLandMarkInSeconds, this.LandmarkInSeconds);
            this.LandmarkInSeconds = newLandMarkInSeconds;
        }

        private double Weight(long timestampInSeconds, long landmarkInSeconds)
        {
            return Math.Exp(this.Alpha * (timestampInSeconds - landmarkInSeconds));
        }

        #endregion

        #region Internal Classes

        public class DecayCounterSnapshot
        {
            #region Public Methods

            public double Count { get; }

            public double Rate { get; }

            #endregion

            #region Constructors

            [JsonConstructor]
            public DecayCounterSnapshot(double count, double rate)
            {
                this.Count = count;
                this.Rate = rate;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                return StringHelper.Build(this)
                    .Add("count", this.Count)
                    .Add("rate", this.Rate)
                    .ToString();
            }

            #endregion
        }

        #endregion
    }
}
