using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// TODO: In progress for TupleDomain
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="C"></typeparam>
    public abstract class AstVisitor<R, C>
    {
        public R Process(Node node)
        {
            return Process(node, default(C));
        }

        /// <summary>
        /// This is a hack just to compile, it's 
        /// not actually called anywhere yet
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public R Process(Node node, C context)
        {
            return node.Accept(this, context);
        }

        protected virtual R VisitNode(Node node, C context)
        {
            return default(R);
        }
    }
}
