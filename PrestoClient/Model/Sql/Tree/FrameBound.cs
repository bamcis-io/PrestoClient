namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.FrameBound.java
    /// </summary>
    public class FrameBound : Node
    {
        #region Public Properties

        public FrameBoundType Type { get; }

        public Expression Value { get; }

        public Expression OriginalValue { get; }
   
        #endregion

        #region Constructors

        public FrameBound(FrameBoundType type) : this(null, type)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type) : this(location, type, null, null)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type, Expression value) : this(location, type, value, value)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type, Expression value, Expression originalValue) : base(location)
        {
            this.Type = type;
            this.Value = value;
            this.OriginalValue = originalValue;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", this.Type)
                .Add("value", this.Value)
                .ToString();
        }

        #endregion
    }
}
