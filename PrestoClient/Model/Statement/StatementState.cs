using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// The state of the submitted statement. Collected from empircal
    /// tests.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatementState
    {
        /// <summary>
        /// Query has been accepted and is awaiting execution.
        /// </summary>
        QUEUED,

        /// <summary>
        /// Query is being planned.
        /// </summary>
        PLANNING,

        /// <summary>
        /// Query has been scheduled for execution.
        /// </summary>
        SCHEDULED,

        /// <summary>
        /// Query execution is being started.
        /// </summary>
        STARTING,

        /// <summary>
        /// Query has at least one running task.
        /// </summary>
        RUNNING,

        /// <summary>
        /// Query is blocked and is waiting for resources (buffer space, memory, splits, etc.).
        /// This is defined in the web-interface.rst file, but not in the QueryState class
        /// </summary>
        BLOCKED,

        /// <summary>
        /// Query is finishing (e.g. commit for autocommit queries).
        /// </summary>
        FINISHING,

        /// <summary>
        /// Query has finished executing and all output has been consumed.
        /// </summary>
        FINISHED,

        /// <summary>
        /// Query execution failed.
        /// </summary>
        FAILED
    }
}