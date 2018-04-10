using System.Linq;

namespace BAMCIS.PrestoClient.Model.Execution
{
    public static class StageStateExtensionMethods
    {
        public static bool IsDone(this StageState state)
        {
            StageState[] DoneStates = new StageState[] { StageState.FINISHED, StageState.CANCELED, StageState.ABORTED, StageState.FAILED };

            return DoneStates.Contains(state);
        }
    }
}
