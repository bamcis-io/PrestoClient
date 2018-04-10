using BAMCIS.PrestoClient.Model.Sql.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Sql
{
    /// <summary>
    /// From com.facebook.presto.sql.ExpressionFormatter.java
    /// </summary>
    public class ExpressionFormatter
    {
        public static string FormatExpression(Expression expression, IEnumerable<Expression> parameters )
        {
            return new Formatter(parameters).Process(expression, null);
        }

        public class Formatter : AstVisitor<string, object>
        {
            public IEnumerable<Expression> Parameters { get; }

            public Formatter(IEnumerable<Expression> parameters)
            {
                this.Parameters = parameters;
            }

            protected override string VisitNode(Node node, object context)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
