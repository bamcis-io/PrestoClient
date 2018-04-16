namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com;facebook.presto.sql.tree.AstVisitor.java
    /// 
    /// TODO: Not completely implemented, just enough to support Expression
    /// string formatting
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="C"></typeparam>
    public abstract class AstVisitor<R, C>
    {
        public R Process(Node node)
        {
            return Process(node, default(C));
        }

        public R Process(Node node, C context)
        {
            return node.Accept(this, context);
        }

        internal protected virtual R VisitNode(Node node, C context)
        {
            return default(R);
        }

        internal protected virtual R VisitExpression(Expression node, C context)
        {
            return VisitNode(node, context);
        }
    }
}
