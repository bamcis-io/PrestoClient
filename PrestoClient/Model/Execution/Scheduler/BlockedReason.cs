namespace BAMCIS.PrestoClient.Model.Execution.Scheduler
{
    public enum BlockedReason
    {
        /// <summary>
        /// Cannot be combined
        /// </summary>
        WRITER_SCALING,

        NO_ACTIVE_DRIVER_GROUP,

        SPLIT_QUEUES_FULL,

        WAITING_FOR_SOURCE,

        MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE
    }
}
