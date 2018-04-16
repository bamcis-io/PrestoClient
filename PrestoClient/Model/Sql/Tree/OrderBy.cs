using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.OrderBy.java
    /// </summary>
    public class OrderBy : Node
    {
        #region Public Properties

        public IEnumerable<SortItem> SortItems { get; }

        #endregion

        #region Constructors

        public OrderBy(IEnumerable<SortItem> sortItems) : this(null, sortItems)
        { }

        [JsonConstructor]
        public OrderBy(NodeLocation location, IEnumerable<SortItem> sortItems) : base(location)
        {
            this.SortItems = sortItems ?? throw new ArgumentNullException("sortItems");
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

            OrderBy Other = (OrderBy)obj;
            return Object.Equals(this.SortItems, Other.SortItems);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.SortItems);
        }

        public override IEnumerable<Node> GetChildren()
        {
            return this.SortItems;
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("sortItems", this.SortItems)
                .ToString();
        }

        #endregion
    }
}
