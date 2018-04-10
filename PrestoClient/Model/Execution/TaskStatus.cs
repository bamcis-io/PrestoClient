using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskStatus.java
    /// </summary>
    public class TaskStatus
    {

        #region Private Fields

        /// <summary>
        /// The first valid version that will be returned for a remote task.
        /// </summary>
        public static readonly long STARTING_VERSION = 1;

        /// <summary>
        /// A value lower than {@link #STARTING_VERSION}. This value can be used to
        /// create an initial local task that is always older than any remote task.
        /// </summary>
        private static readonly long MIN_VERSION = 0;

        /// <summary>
        /// A value larger than any valid value. This value can be used to create
        /// a final local task that is always newer than any remote task.
        /// </summary>
        private static readonly long MAX_VERSION = Int64.MaxValue;

        #endregion

        #region Public Properties

        public TaskId TaskId { get; }

        public Guid TaskInstanceId { get; }

        public long Version { get; }

        public TaskState State { get; }

        public Uri Self { get; }

        public Guid NodeId { get; }

        public HashSet<Lifespan> CompletedDriverGroups { get; }

        public IEnumerable<ExecutionFailureInfo> Failures { get; }

        public int QueuedPartitionedDrivers { get; }

        public int RunningPartitionedDrivers { get; }

        public bool OutputBufferOverutilized { get; }

        public DataSize PhysicalWrittenDataSize { get; }

        public DataSize MemoryReservation { get; }

        public DataSize SystemMemoryReservation { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TaskStatus (
            TaskId taskId,
            Guid taskInstanceId,
            long version,
            TaskState state,
            Uri self,
            Guid nodeId,
            HashSet<Lifespan> completedDriverGroups,
            IEnumerable<ExecutionFailureInfo> failures,
            int queuedPartitionDrivers,
            int runningPartitionDrivers,
            bool outputBufferOverutilized,
            DataSize physicalWrittenDataSize,
            DataSize memoryReservation,
            DataSize systemMemoryReservation
            )
        {
            ParameterCheck.OutOfRange(version >= MIN_VERSION, "version", $"The version cannot be less than the minimum version of {MIN_VERSION}.");
            ParameterCheck.OutOfRange(queuedPartitionDrivers >= 0, "queuedPartitionDrivers", "The queued partition drivers cannot be less than 0.");
            ParameterCheck.OutOfRange(runningPartitionDrivers >= 0, "runningPartitionDrivers", "The running partition drivers cannot be less than 0.");

            this.TaskId = taskId ?? throw new ArgumentNullException("taskId");
            this.TaskInstanceId = taskInstanceId;
            this.Version = version;
            this.State = state;
            this.Self = self ?? throw new ArgumentNullException("self");
            this.NodeId = nodeId;
            this.CompletedDriverGroups = completedDriverGroups ?? throw new ArgumentNullException("completedDriverGroups");

            this.QueuedPartitionedDrivers = queuedPartitionDrivers;
            this.RunningPartitionedDrivers = runningPartitionDrivers;
            this.OutputBufferOverutilized = outputBufferOverutilized;
            this.PhysicalWrittenDataSize = physicalWrittenDataSize ?? throw new ArgumentNullException("physicalWrittenDataSize");
            this.MemoryReservation = memoryReservation ?? throw new ArgumentNullException("memoryReservation");
            this.SystemMemoryReservation = systemMemoryReservation ?? throw new ArgumentNullException("systemMemoryReservation");
            this.Failures = failures ?? throw new ArgumentNullException("failures");
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("taskId", this.TaskId)
                .Add("state", this.State)
                .ToString();
        }

        public static TaskStatus InitialTaskStatus(TaskId taskId, Uri location, string nodeId)
        {
            return new TaskStatus(
                taskId,
                Guid.Empty,
                MIN_VERSION,
                TaskState.PLANNED,
                location,
                 Guid.Parse(nodeId),
                 new HashSet<Lifespan>(),
                 new ExecutionFailureInfo[0],
                 0,
                 0,
                 false,
                 new DataSize(0, DataSizeUnit.BYTE),
                 new DataSize(0, DataSizeUnit.BYTE),
                 new DataSize(0, DataSizeUnit.BYTE)
            );
        }

        public static TaskStatus FailWith(TaskStatus taskStatus, TaskState state, List<ExecutionFailureInfo> exceptions)
        {
            return new TaskStatus(
                taskStatus.TaskId,
                taskStatus.TaskInstanceId,
                MAX_VERSION,
                state,
                taskStatus.Self,
                taskStatus.NodeId,
                taskStatus.CompletedDriverGroups,
                exceptions,
                taskStatus.QueuedPartitionedDrivers,
                taskStatus.RunningPartitionedDrivers,
                taskStatus.OutputBufferOverutilized,
                taskStatus.PhysicalWrittenDataSize,
                taskStatus.MemoryReservation,
                taskStatus.SystemMemoryReservation
            );
        }

        #endregion
    }
}
