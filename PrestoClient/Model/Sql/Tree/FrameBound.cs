using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.FrameBound.java
    /// </summary>
    public class FrameBound : Node
    {
        #region Public Properties

        public FrameBoundType Type { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic Value { get; }

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic OriginalValue { get; }
   
        #endregion

        #region Constructors

        public FrameBound(FrameBoundType type) : this(null, type)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type) : this(location, type, null, null)
        { }

        public FrameBound(NodeLocation location, FrameBoundType type, dynamic value) : this(location, type, (object)value, (object)value)
        { }

        [JsonConstructor]
        public FrameBound(NodeLocation location, FrameBoundType type, dynamic value, dynamic originalValue) : base(location)
        {
            this.Type = type;
            this.Value = value;
            this.OriginalValue = originalValue;
        }

        #endregion

        #region Public Methods

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
            FrameBound Other = (FrameBound)obj;

            return Object.Equals(this.Type, Other.Type) &&
                    Object.Equals(this.Value, Other.Value);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this.Value);
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return this.Value;
        }

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
