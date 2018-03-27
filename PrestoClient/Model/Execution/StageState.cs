namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageState.java
    /// </summary>
    public enum StageState
    {
        /// <summary>
        /// Stage is planned but has not been scheduled yet.  A stage will
        /// be in the planned state until, the dependencies of the stage
        /// have begun producing output.
        /// </summary>
        PLANNED,

        /// <summary>
        /// Stage tasks are being scheduled on nodes.
        /// </summary>
        SCHEDULING,

        /// <summary>
        /// All stage tasks have been scheduled, but splits are still being scheduled.
        /// </summary>
        SCHEDULING_SPLITS,

        /// <summary>
        /// Stage has been scheduled on nodes and ready to execute, but all tasks are still queued.
        /// </summary>
        SCHEDULED,

        /// <summary>
        /// Stage is running.
        /// </summary>
        RUNNING,

        /// <summary>
        /// Stage has finished executing and all output has been consumed.
        /// </summary>
        FINISHED,

        /// <summary>
        /// Stage was canceled by a user.
        /// </summary>
        CANCELED,

        /// <summary>
        /// Stage was aborted due to a failure in the query.  The failure
        /// was not in this stage.
        /// </summary>
        ABORTED,

        /// <summary>
        /// tage execution failed.
        /// </summary>
        FAILED
    }
}
