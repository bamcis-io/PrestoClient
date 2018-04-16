using BAMCIS.PrestoClient.Model.Sql.Tree;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql
{
    /// <summary>
    /// From com.facebook.presto.sql.ExpressionFormatter.java
    /// 
    /// TODO: In progress for Expression, all that is needed is the FormatExpression method
    /// to do string conversion
    /// </summary>
    public class ExpressionFormatter
    {
        #region Constructors

        private ExpressionFormatter()
        { }

        #endregion

        #region Public Static Methods

        public static string FormatExpression(Expression expression, IEnumerable<Expression> parameters )
        {
            return new Formatter(parameters).Process(expression, null);
        }

        #endregion

        public class Formatter : AstVisitor<string, object>
        {
            public IEnumerable<Expression> Parameters { get; }

            public Formatter(IEnumerable<Expression> parameters)
            {
                this.Parameters = parameters;
            }

            internal protected override string VisitNode(Node node, object context)
            {
                throw new InvalidOperationException();
            }

            internal protected override string VisitExpression(Expression node, object context)
            {
                throw new InvalidOperationException($"Not yet implemented: {this.GetType().Name}.visit{node.GetType().Name}.");
            }
        }
    }
}
