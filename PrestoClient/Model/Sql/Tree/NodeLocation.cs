using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.NodeLocation.java
    /// </summary>
    public sealed class NodeLocation
    {
        #region Public Properties

        public int Line { get; }

        [JsonProperty(PropertyName = "columnNumber")]
        public int CharPositionInLine { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public NodeLocation(int line, int columnNumber)
        {
            this.Line = line;
            this.CharPositionInLine = columnNumber;
        }

        #endregion
    }
}
