namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.Expression.java
    /// </summary>
    public abstract class Expression : Node
    {
        #region Constructors

        protected Expression(NodeLocation location) : base(location)
        { }

        #endregion
    }
}
