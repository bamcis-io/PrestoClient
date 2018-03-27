using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Node
{
    /// <summary>
    /// Represents failure information reported by a presto node.
    /// </summary>
    public class NodeFailureInfo
    {
        /// <summary>
        /// The failure message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The stack trace from the failure.
        /// </summary>
        public IEnumerable<string> Stack { get; set; }

        /// <summary>
        /// Any suppressed errors.
        /// </summary>
        public IEnumerable<string> Suppressed { get; set; }

        /// <summary>
        /// The type of the exception
        /// </summary>
        public string Type { get; set; }
    }
}