using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.SortItem.java
    /// </summary>
    public class SortItem : Node
    {
        #region Public Properties

        public Expression SortKey { get; }

        public Ordering Ordering { get; }

        public NullOrdering NullOrdering { get; }

        #endregion

        #region Constructors

        public SortItem(Expression sortKey, Ordering ordering, NullOrdering nullOrdering) : this(null, sortKey, ordering, nullOrdering)
        { }

        public SortItem(NodeLocation location, Expression sortKey, Ordering ordering, NullOrdering nullOrdering) : base(location)
        {
            this.Ordering = ordering;
            this.NullOrdering = nullOrdering;
            this.SortKey = sortKey;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Node> GetChildren()
        {
            return new Node[] { this.SortKey };
        }

        #endregion
    }
}
