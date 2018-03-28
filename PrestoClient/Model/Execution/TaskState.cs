using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskState.java
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskState
    {
        /// <summary>
        /// Task is planned but has not been scheduled yet.  A task will
        /// be in the planned state until, the dependencies of the task
        /// have begun producing output.
        /// </summary>
        PLANNED,

        /// <summary>
        /// Task is running.
        /// </summary>
        RUNNING,

        /// <summary>
        /// Task has finished executing and all output has been consumed.
        /// </summary>
        FINISHED,

        /// <summary>
        /// Task was canceled by a user.
        /// </summary>
        CANCELED,

        /// <summary>
        /// Task was aborted due to a failure in the query.  The failure
        /// was not in this task.
        /// </summary>
        ABORTED,

        /// <summary>
        /// Task execution failed.
        /// </summary>
        FAILED
    }
}
