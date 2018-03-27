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
    }
}