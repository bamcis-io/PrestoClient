using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.SortItem.java
    /// </summary>
    public class SortItem : Node
    {
        #region Public Properties

        /// <summary>
        /// TODO: Supposed to be Expression
        /// </summary>
        public dynamic SortKey { get; }

        public Ordering Ordering { get; }

        public NullOrdering NullOrdering { get; }

        #endregion

        #region Constructors

        public SortItem(object sortKey, Ordering ordering, NullOrdering nullOrdering) : this(null, sortKey, ordering, nullOrdering)
        { }

        public SortItem(NodeLocation location, object sortKey, Ordering ordering, NullOrdering nullOrdering) : base(location)
        {
            this.Ordering = ordering;
            this.NullOrdering = nullOrdering;
            this.SortKey = sortKey;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            SortItem Other = (SortItem)obj;

            return Object.Equals(this.SortKey, Other.SortKey) &&
                    (this.Ordering == Other.Ordering) &&
                    (this.NullOrdering == Other.NullOrdering);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.SortKey, this.Ordering, this.NullOrdering);
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("sortKey", this.SortKey)
                .Add("ordering", this.Ordering)
                .Add("nullOrdering", this.NullOrdering)
                .ToString();
        }

        public override IEnumerable<Node> GetChildren()
        {
            return new Node[] { this.SortKey };
        }

        #endregion
    }
}
