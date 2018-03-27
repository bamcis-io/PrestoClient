using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.PlatFlattener.java (internal class FlattendedNode)
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class FlattenedNode
    {
        #region Public Properties

        public string Node { get; }

        #endregion

        #region Constructors

        public FlattenedNode(string node)
        {
            if (String.IsNullOrEmpty(node))
            {
                throw new ArgumentNullException("node", "The node cannot be null or empty");
            }

            this.Node = node;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Node;
        }

       

        #endregion
    }
}
