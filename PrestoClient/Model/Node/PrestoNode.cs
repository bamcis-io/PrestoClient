using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Node
{
    /// <summary>
    /// Information about a Presto node
    /// </summary>
    public class PrestoNode
    {
        #region Public Properties

        /// <summary>
        /// The URI to the node
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 requests in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentRequests { get; set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 failures in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentFailuers { get; set; }

        /// <summary>
        /// A decaying counter set to decay exponentially over time with a decay parameter of one minute. This  
        /// decay rate means that if a Presto node has 1000 successes in a few
        /// seconds, this statistic value will drop to 367 in one minute
        /// </summary>
        public double RecentSuccesses { get; set; }

        /// <summary>
        /// The time of the last request.
        /// </summary>
        public DateTime LastRequestTime { get; set; }

        /// <summary>
        /// The time of the last response.
        /// </summary>
        public DateTime LastResponseTime { get; set; }

        /// <summary>
        /// The ratio of failures to success.
        /// </summary>
        public double RecentFailureRatio { get; set; }

        /// <summary>
        /// The length of time the node has been up.
        /// </summary>
        [JsonConverter(typeof(TimespanConverter))]
        public TimeSpan Age { get; set; }

        /// <summary>
        /// Recent failures on the node.
        /// </summary>
        public IDictionary<string, double> RecentFailuresByType { get; set; }

        /// <summary>
        /// The last failure information.
        /// </summary>
        public NodeFailureInfo LastFailureInfo { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new presto node information object
        /// with LastFailureInfo set to null and an empty
        /// RecentFailuresByType property. The rest of the
        /// properties are default values.
        /// </summary>
        public PrestoNode()
        {
            LastFailureInfo = null;
            RecentFailuresByType = new Dictionary<string, double>();
        }

        #endregion
    }
}