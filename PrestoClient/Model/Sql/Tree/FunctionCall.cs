using Newtonsoft.Json;
using System;
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

        #region Public Properties

        public override IEnumerable<Node> GetChildren()
        {
            if (this.Window != null)
            {
                yield return this.Window;
            }

            if (this.Filter != null)
            {
                yield return this.Filter;
            }

            foreach (SortItem Item in this.OrderBy.SortItems)
            {
                yield return Item;
            }

            foreach (Expression Item in this.Arguments)
            {
                yield return Item;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if ((obj == null) || (this.GetType() != obj.GetType()))
            {
                return false;
            }

            FunctionCall Other = (FunctionCall)obj;

            return object.Equals(this.Name, Other.Name) &&
                    object.Equals(this.Window, Other.Window) &&
                    object.Equals(this.Filter, Other.Filter) &&
                    object.Equals(this.OrderBy, Other.OrderBy) &&
                    object.Equals(this.Distinct, Other.Distinct) &&
                    object.Equals(this.Arguments, Other.Arguments);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Name, this.Distinct, this.Window, this.Filter, this.OrderBy, this.Arguments);
        }

        #endregion
    }
}
