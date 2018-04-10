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

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Filter { get; }

        public OrderBy OrderBy { get; }

        public bool Distinct { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public IEnumerable<dynamic> Arguments { get; }

        #endregion

        #region Constructors

        public FunctionCall(QualifiedName name, IEnumerable<object> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(NodeLocation location, QualifiedName name, IEnumerable<object> arguments)
            : this(null, name, null, null, null, false, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<object> arguments)
            : this(null, name, null, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, bool distinct, IEnumerable<object> arguments, object filter)
            : this(null, name, null, filter, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, bool distinct, IEnumerable<object> arguments)
            : this(null, name, window, null, null, distinct, arguments)
        { }

        public FunctionCall(QualifiedName name, Window window, object filter, OrderBy orderBy, bool distinct, IEnumerable<object> arguments) 
            : this(null, name, window, filter, orderBy, distinct, arguments)
        { }

        [JsonConstructor]
        public FunctionCall(NodeLocation location, QualifiedName name, Window window, object filter, OrderBy orderBy, bool distinct, IEnumerable<object> arguments) : base(location)
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
