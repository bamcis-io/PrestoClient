using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.FailureDetector
{
    /// <summary>
    /// Stats about the heartbeat of a Presto node
    /// From com.facebook.presto.failureDetector.HeartbeatFailureDetector.java (internal class Stats)
    /// 
    /// This class provides mostly a holder for data than a direct port of the code since the actual 
    /// values are generated from the decay counters, which are not set from the json deserialization.
    /// </summary>
    public class HeartbeatFailureDetectorStats
    {
        #region Private Fields

        private readonly DecayCounter _RecentRequests = new DecayCounter(ExponentialDecay.OneMinute());
        private readonly DecayCounter _RecentFailures = new DecayCounter(ExponentialDecay.OneMinute());
        private readonly DecayCounter _RecentSuccesses = new DecayCounter(ExponentialDecay.OneMinute());

        #endregion

        #region Public Properties

        /// <summary>
        /// The URI to the node
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 requests in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentRequests { get; private set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 failures in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentFailures { get; private set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 successes in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentSuccesses { get; private set; }

        /// <summary>
        /// The time of the last request.
        /// </summary>
        public DateTime LastRequestTime { get; private set; }

        /// <summary>
        /// The time of the last response.
        /// </summary>
        public DateTime LastResponseTime { get; private set; }

        /// <summary>
        /// The ratio of failures to success.
        /// </summary>
        public double RecentFailureRatio { get; private set; }

        /// <summary>
        /// The length of time the node has been up.
        /// </summary>
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Age { get; private set; }

        /// <summary>
        /// Recent failures on the node.
        /// </summary>
        public IDictionary<string, double> RecentFailuresByType { get; private set; }

        /// <summary>
        /// The last failure information.
        /// </summary>
        public FailureInfo LastFailureInfo { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new presto node information object
        /// with LastFailureInfo set to null and an empty
        /// RecentFailuresByType property. The rest of the
        /// properties are default values.
        /// </summary>
        public HeartbeatFailureDetectorStats()
        {
            LastFailureInfo = null;
            RecentFailuresByType = new Dictionary<string, double>();
        }

        /// <summary>
        /// Creates a new presto node information object
        /// with the supplied Uri.
        /// </summary>
        /// <param name="uri"></param>
        [JsonConstructor]
        public HeartbeatFailureDetectorStats(Uri uri) : this()
        {
            this.Uri = uri;
        }

        #endregion

        #region Public Methods

        public void RecordStart()
        {
            this._RecentRequests.Add(1);
            this.LastRequestTime = new DateTime();
        }

        public void RecordSuccess()
        {
            this._RecentSuccesses.Add(1);
            this.LastResponseTime = new DateTime();
        }

        #endregion
    }
}