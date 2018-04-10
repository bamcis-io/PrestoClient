using BAMCIS.PrestoClient.Model.Execution.Buffer;
using BAMCIS.PrestoClient.Model.Operator;
using BAMCIS.PrestoClient.Model.Sql.Planner.Plan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.TaskInfo.java
    /// </summary>
    public class TaskInfo
    {
        #region Public Properties

        public TaskStatus TaskStatus { get; }

        public DateTime LastHeartbeat { get; }

        public OutputBufferInfo OutputBuffers { get; }

        public HashSet<PlanNodeId> NoMoreSplits { get; }

        public TaskStats Stats { get; }

        public bool NeedsPlan { get; }

        public bool Complete { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TaskInfo(
            TaskStatus taskStatus,
            DateTime lastHeartbeat,
            OutputBufferInfo outputBuffers,
            HashSet<PlanNodeId> noMoreSplits,
            TaskStats stats,
            bool needsPlan,
            bool complete
            )
        {
            this.TaskStatus = taskStatus ?? throw new ArgumentNullException("taskStatus");
            this.LastHeartbeat = lastHeartbeat;
            this.OutputBuffers = outputBuffers ?? throw new ArgumentNullException("outputBuffers");
            this.NoMoreSplits = noMoreSplits ?? throw new ArgumentNullException("noMoreSplits");
            this.Stats = stats ?? throw new ArgumentNullException("stats");

            this.NeedsPlan = needsPlan;
            this.Complete = complete;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("taskId", this.TaskStatus.TaskId)
                .Add("state", this.TaskStatus.State.ToString())
                .ToString();
        }

        public static TaskInfo CreateInitialTask(TaskId taskId, Uri location, string nodeId, IEnumerable<BufferInfo> bufferStates, TaskStats taskStats)
        {
            return new TaskInfo(
                    TaskStatus.InitialTaskStatus(taskId, location, nodeId),
                    DateTime.Now,
                    new OutputBufferInfo("UNINITIALIZED", BufferState.OPEN, true, true, 0, 0, 0, 0, bufferStates),
                    new HashSet<PlanNodeId>(),
                    taskStats,
                    true,
                    false
                );
        }

        public TaskInfo WithTaskStatus(TaskStatus newTaskStatus)
        {
            return new TaskInfo(newTaskStatus, this.LastHeartbeat, this.OutputBuffers, this.NoMoreSplits, this.Stats, this.NeedsPlan, this.Complete);
        }

        #endregion
    }
}
