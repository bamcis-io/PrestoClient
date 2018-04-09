using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Frame)
    /// </summary>
    public class Frame
    {
        #region Public Properties

        public WindowFrameType Type { get; }

        public FrameBoundType StartType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol StartValue { get; }

        public FrameBoundType EndType { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public Symbol EndValue { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Frame(WindowFrameType type, FrameBoundType startType, Symbol startValue, FrameBoundType endType, Symbol endValue)
        {
            this.Type = type;
            this.StartType = startType;
            this.StartValue = startValue;
            this.EndType = endType;
            this.EndValue = endValue;
        }

        #endregion

        #region Public Methods

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() +
                this.StartType.GetHashCode() +
                this.EndType.GetHashCode() +
                ((this.StartValue != null) ? this.StartValue.GetHashCode() : 0) +
                ((this.EndValue != null) ? this.EndValue.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Frame) != obj.GetType())
            {
                return false;
            }

            Frame other = (Frame)obj;

            return object.Equals(this.Type, other.Type) &&
                    object.Equals(this.StartType, other.StartType) &&
                    object.Equals(this.StartValue, other.StartValue) &&
                    object.Equals(this.EndType, other.EndType) &&
                    object.Equals(this.EndValue, other.EndValue);
        }

        #endregion
    }
}
