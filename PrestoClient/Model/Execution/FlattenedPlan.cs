using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// from com.facebook.execution.PlatFlanner.java (internal call FlattendedPlan)
    /// </summary>
    public class FlattenedPlan
    {
        #region Public Properties

        public IEnumerable<FlattenedPlanFragment> Fragments { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public FlattenedPlan(IEnumerable<FlattenedPlanFragment> fragments)
        {
            this.Fragments = fragments;
        }

        #endregion
    }
}
