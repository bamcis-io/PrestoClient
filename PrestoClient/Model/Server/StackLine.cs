using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.Server
{
    /// <summary>
    /// From com.facebook.presto.server.ThreadResource.java (internal class StackLine)
    /// </summary>
    public class StackLine
    {
        #region Public Properties

        public string File { get; }

        public int Line { get; }

        public string Method { get; }

        public string ClassName { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public StackLine(string file, int line, string className, string method)
        {
            this.File = file;
            this.Line = line;
            this.ClassName = className;
            this.Method = method;
        }

        #endregion
    }
}
