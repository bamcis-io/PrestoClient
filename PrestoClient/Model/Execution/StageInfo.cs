using BAMCIS.PrestoClient.Model.Sql.Planner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageInfo.java
    /// </summary>
    public class StageInfo
    {
        #region Public Properties

        public StageId StageId { get; }

        public StageState State { get; }

        public Uri Self { get; }

        public PlanFragment Plan { get; }

        /// <summary>
        /// Should be IType interface
        /// </summary>
        public IEnumerable<string> Types { get; }

        public StageStats StageStats { get; }

        public IEnumerable<TaskInfo> Tasks { get; }

        public IEnumerable<StageInfo> SubStages { get; }

        public ExecutionFailureInfo FailureCause { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StageInfo(
            StageId stageId,
            StageState state,
            Uri self,
            PlanFragment plan,
            IEnumerable<string> types,
            StageStats stageStats,
            IEnumerable<TaskInfo> tasks,
            IEnumerable<StageInfo> subStages,
            ExecutionFailureInfo failureCause
            )
        {
            this.StageId = stageId ?? throw new ArgumentNullException("stageId");
            this.State = state;
            this.Self = self ?? throw new ArgumentNullException("self");
            this.Plan = plan;
            this.Types = types;
            this.StageStats = stageStats ?? throw new ArgumentNullException("stageStats");
            this.Tasks = tasks ?? throw new ArgumentNullException("tasks");
            this.SubStages = subStages ?? throw new ArgumentNullException("subStages");
            this.FailureCause = failureCause;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("stageId", this.StageId)
                .Add("state", this.State.ToString())
                .ToString();
        }

        public bool IsCompleteInfo()
        {
            return this.State.IsDone() && this.Tasks.All(x => x.Complete);
        }

        #endregion
    }
}
