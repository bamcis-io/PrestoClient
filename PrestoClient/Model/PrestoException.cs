using System;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// A custom exception to wrap presto API errors
    /// </summary>
    public class PrestoException : Exception
    {
        #region Public Properties

        /// <summary>
        /// The raw representation of the data returned by presto
        /// </summary>
        public string RawResponseContent { get; }

        #endregion

        #region Constructors

        public PrestoException(string message) : base(message)
        {
            this.RawResponseContent = String.Empty;
        }

        public PrestoException(string message, Exception innerException) : base(message, innerException)
        {
            this.RawResponseContent = String.Empty;
        }

        public PrestoException(string message, string rawContent) : base(message)
        {
            this.RawResponseContent = rawContent;
        }

        public PrestoException(string message, string rawContent, Exception innerException) : base(message, innerException)
        {
            this.RawResponseContent = rawContent;
        }

        #endregion
    }
}