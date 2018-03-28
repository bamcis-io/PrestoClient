using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.QueryState and
    /// https://github.com/prestodb/presto/blob/c73359fe2173e01140b7d5f102b286e81c1ae4a8/presto-docs/src/main/sphinx/admin/web-interface.rst
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QueryState
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