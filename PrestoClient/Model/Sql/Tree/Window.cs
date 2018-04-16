using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.Window.java
    /// </summary>
    public class Window : Node
    {
        #region Public Properties

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public IEnumerable<dynamic> PartitionBy { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public OrderBy OrderBy { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public WindowFrame Frame { get; }

        #endregion

        #region Constructors

        public Window(IEnumerable<object> partitionBy, OrderBy orderBy, WindowFrame frame) : this(null, partitionBy, orderBy, frame)
        {
        }

        [JsonConstructor]
        public Window(NodeLocation location, IEnumerable<object> partitionBy, OrderBy orderBy, WindowFrame frame) : base(location)
        {
            this.PartitionBy = partitionBy ?? throw new ArgumentNullException("partitionBy");
            this.OrderBy = orderBy; // These are actually optional ?? throw new ArgumentNullException("orderBy");
            this.Frame = frame; // These are actually optional ?? throw new ArgumentNullException("frame");
        }

        #endregion

        #region Public Properties

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

            Window Other = (Window)obj;

            return Object.Equals(this.PartitionBy, Other.PartitionBy) &&
                    Object.Equals(this.OrderBy, Other.OrderBy) &&
                    Object.Equals(this.Frame, Other.Frame);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.PartitionBy, this.OrderBy, this.Frame);
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("partitionBy", this.PartitionBy)
                .Add("orderBy", this.OrderBy)
                .Add("frame", this.Frame)
                .ToString();
        }

        public override IEnumerable<Node> GetChildren()
        {
            foreach (Node Item in this.PartitionBy)
            {
                yield return Item;
            }

            if (this.OrderBy != null)
            {
                yield return this.OrderBy;
            }

            if (this.Frame != null)
            {
                yield return this.Frame;
            }
        }

        #endregion
    }
}
