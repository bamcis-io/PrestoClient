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

        public IEnumerable<Expression> PartitionBy { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public OrderBy OrderBy { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public WindowFrame Frame { get; }

        #endregion

        #region Constructors

        public Window(IEnumerable<Expression> partitionBy, OrderBy orderBy, WindowFrame frame) : this(null, partitionBy, orderBy, frame)
        {
        }

        [JsonConstructor]
        public Window(NodeLocation location, IEnumerable<Expression> partitionBy, OrderBy orderBy, WindowFrame frame) : base(location)
        {
            this.PartitionBy = partitionBy ?? throw new ArgumentNullException("partitionBy");
            this.OrderBy = orderBy; // These are actually optional ?? throw new ArgumentNullException("orderBy");
            this.Frame = frame; // These are actually optional ?? throw new ArgumentNullException("frame");
        }

        #endregion

        #region Public Properties

        public IEnumerable<Node> GetChildren()
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
