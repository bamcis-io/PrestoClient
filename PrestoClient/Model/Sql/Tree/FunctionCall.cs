using Newtonsoft.Json;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.FunctionCall.java
    /// </summary>
    public class FunctionCall : Expression
    {
        #region Public Properties

        public QualifiedName Name { get; }

        public Window Window { get; }

        public Expression Filter { get; }

        public OrderBy OrderBy { get; }

        public bool Distinct { get; }

        public IEnumerable<Expression> Arguments { get; }

        #endregion

        #region Constructors

        public FunctionCall(QualifiedName name, IEnumerable<Expression> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(NodeLocation location, QualifiedName name, IEnumerable<Expression> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<Expression> arguments)
            : this(null, name, null, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<Expression> arguments, Expression filter)
            : this(null, name, null, filter, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, bool distinct, IEnumerable<Expression> arguments)
            : this(null, name, window, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, Expression filter, OrderBy orderBy, bool distinct, IEnumerable<Expression> arguments) 
            : this(null, name, window, filter, orderBy, distinct, arguments)
        { }

        [JsonConstructor]
        public FunctionCall(NodeLocation location, QualifiedName name, Window window, Expression filter, OrderBy orderBy, bool distinct, IEnumerable<Expression> arguments) : base(location)
        {
            this.Name = name;
            this.Window = window;
            this.Filter = filter;
            this.OrderBy = orderBy;
            this.Distinct = distinct;
            this.Arguments = arguments;
        }

        #endregion
    }
}
