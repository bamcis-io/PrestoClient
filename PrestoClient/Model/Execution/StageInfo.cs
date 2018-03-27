using BAMCIS.PrestoClient.Model.Sql.Planner;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.StageInfo.java
    /// </summary>
    public class StageInfo
    {
        public StageId StageId { get; set; }

        public StageState State { get; set; }

        public Uri Self { get; set; }

        public PlanFragment Plan { get; set; }

        /// <summary>
        /// Should be IType interface
        /// </summary>
        public IEnumerable<string> Types { get; set; }

        public StageStats StageStats { get; set; }

        public IEnumerable<TaskInfo> Tasks { get; set; }

        public IEnumerable<StageInfo> SubStages { get; set; }

        public ExecutionFailureInfo FailureCause { get; set; }
    }
}
