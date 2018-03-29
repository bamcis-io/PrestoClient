using System;
using System.Net;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// A custom exception to wrap presto API errors
    /// </summary>
    public class PrestoException : Exception
    {
        /// <summary>
        /// The return status code of the request that failed
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The raw representation of the data returned by presto
        /// </summary>
        public string RawResponseContent { get; private set; }

        public PrestoException(string message) : base(message)
        {
        }

        public PrestoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PrestoException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public PrestoException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        public PrestoException(string message, string rawContent) : base(message)
        {
            this.RawResponseContent = rawContent;
        }

        public PrestoException(string message, string rawContent, HttpStatusCode statusCode) : base(message)
        {
            this.StatusCode = statusCode;
            this.RawResponseContent = rawContent;
        }

        public PrestoException(string message, string rawContent, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            this.StatusCode = statusCode;
            this.RawResponseContent = rawContent;
        }

        public PrestoException(string message, string rawContent, Exception innerException) : base(message, innerException)
        {
            this.RawResponseContent = rawContent;
        }
    }
}