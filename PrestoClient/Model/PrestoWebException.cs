using System;
using System.Net;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// Represents an exception that occurs during a web API call to Presto
    /// </summary>
    public class PrestoWebException : PrestoException
    {
        #region Public Properties

        /// <summary>
        /// The return status code of the request that failed
        /// </summary>
        public HttpStatusCode StatusCode { get; }


        #endregion

        #region Constructors

        public PrestoWebException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public PrestoWebException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }

        public PrestoWebException(string message, string rawContent, HttpStatusCode statusCode) : base(message, rawContent)
        {
            this.StatusCode = statusCode;
        }

        public PrestoWebException(string message, string rawContent, HttpStatusCode statusCode, Exception innerException) : base(message, rawContent, innerException)
        {
            this.StatusCode = statusCode;
        }
    }

    #endregion
}