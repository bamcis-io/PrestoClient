using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.Node.java
    /// </summary>
    public abstract class Node
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public NodeLocation Location { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        protected Node(NodeLocation location)
        {
            this.Location = location;
        }

        #endregion
    }
}
