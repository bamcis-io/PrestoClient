using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Query
{
    public class QueryStageTaskStatus
    {
        public string TaskId { get; set; }

        public Guid TaskInstanceId { get; set; }

        public int Version { get; set; }

        public QueryState State { get; set; }

        public Uri Self { get; set; }

        public Guid NodeId { get; set; }

        /// <summary>
        /// TODO: Data example unknown
        /// </summary>
        public IEnumerable<object> CompletedDriverGroups { get; set; }

        /// <summary>
        /// TODO: Data example unknown
        /// </summary>
        public IEnumerable<object> Failures { get; set; }

        public int QueuedPartitionedDrivers { get; set; }

        public int RunningPartitionedDrivers { get; set; }

        public bool OutputBufferOverutilized { get; set; }

        public string PhysicalWrittenDataSize { get; set; }

        public string MemoryReservation { get; set; }

        public string SystemMemoryReservation { get; set; }
    }
}
