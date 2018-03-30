using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.WindowFrame.java
    /// </summary>
    public class WindowFrame : Node
    {
        #region Public Properties

        public WindowFrameType Type { get; }

        public FrameBound Start { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public FrameBound End { get; }

        #endregion

        #region Constructors

        public WindowFrame(WindowFrameType type, FrameBound start, FrameBound end) : this(null, type, start, end)
        {
        }

        [JsonConstructor]
        public WindowFrame(NodeLocation location, WindowFrameType type, FrameBound start, FrameBound end) : base(location)
        {
            this.Type = type;
            this.Start = start ?? throw new ArgumentNullException("start");
            this.End = end; // this is optional ?? throw new ArgumentNullException("end")
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("type", this.Type)
                .Add("start", this.Start)
                .Add("end", this.End)
                .ToString();
        }

        #endregion
    }
}
