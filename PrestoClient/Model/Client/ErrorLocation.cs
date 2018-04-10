using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ErrorLocation.java
    /// </summary>
    public class ErrorLocation
    {
        #region Public Properties

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        #endregion

        #region Constructors 

        [JsonConstructor]
        public ErrorLocation(int lineNumber, int columnNumber)
        {
            if (lineNumber < 1)
            {
                throw new ArgumentOutOfRangeException("lineNumber", "The line number must be at least one.");
            }

            if (columnNumber < 1)
            {
                throw new ArgumentOutOfRangeException("columnNumber", "The column number must be at least one.");
            }

            this.LineNumber = lineNumber;
            this.ColumnNumber = columnNumber;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("lineNumber", this.LineNumber)
                .Add("columnNumber", this.ColumnNumber)
                .ToString();
        }

        #endregion
    }
}