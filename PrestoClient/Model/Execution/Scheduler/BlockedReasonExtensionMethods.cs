using System;

namespace BAMCIS.PrestoClient.Model.Execution.Scheduler
{
    public static class BlockedReasonExtensionMethods
    {
        public static BlockedReason CombineWith(this BlockedReason reason, BlockedReason other)
        {
            switch (reason)
            {
                case BlockedReason.WRITER_SCALING:
                    throw new ArgumentException("Cannot be combined");
                case BlockedReason.NO_ACTIVE_DRIVER_GROUP:
                    return other;
                case BlockedReason.SPLIT_QUEUES_FULL:
                    return 
                        other == BlockedReason.SPLIT_QUEUES_FULL || other == BlockedReason.NO_ACTIVE_DRIVER_GROUP ? 
                        BlockedReason.SPLIT_QUEUES_FULL : 
                        BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE;
                case BlockedReason.WAITING_FOR_SOURCE:
                    return 
                        other == BlockedReason.WAITING_FOR_SOURCE || other == BlockedReason.NO_ACTIVE_DRIVER_GROUP ? 
                        BlockedReason.WAITING_FOR_SOURCE :
                        BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE;
                case BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE:
                    return BlockedReason.MIXED_SPLIT_QUEUES_FULL_AND_WAITING_FOR_SOURCE;
                default:
                    throw new ArgumentException($"Unknown blocked reason: {other}.");
            }
        }
    }
}
