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
            return Hashing.Hash(
                this.Type,
                this.StartType,
                this.EndType,
                this.StartValue,
                this.EndValue);
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

            Frame Other = (Frame)obj;

            return object.Equals(this.Type, Other.Type) &&
                    object.Equals(this.StartType, Other.StartType) &&
                    object.Equals(this.StartValue, Other.StartValue) &&
                    object.Equals(this.EndType, Other.EndType) &&
                    object.Equals(this.EndValue, Other.EndValue);
        }

        #endregion
    }
}
