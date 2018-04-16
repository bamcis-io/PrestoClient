using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.NodeLocation.java
    /// </summary>
    public sealed class NodeLocation
    {
        #region Private Fields

        private int CharPositionInLine;

        #endregion

        #region Public Properties

        public int Line { get; }

        public int ColumnNumber
        {
            get
            {
                return this.CharPositionInLine + 1;
            }
        }

        #endregion

        #region Constructors

        [JsonConstructor]
        public NodeLocation(int line, int columnNumber)
        {
            this.Line = line;
            // This is done for serialization purposes, since the serialized
            // data will show the column number, not char position
            this.CharPositionInLine = columnNumber - 1;
        }

        #endregion
    }
}
