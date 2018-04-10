using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// Represents an exception that can be created from a failure reported to the
    /// client
    /// Partially taken from com.facebook.presto.client.FailureInfo.java (internal class FailureException)
    /// </summary>
    public class PrestoFailureException : PrestoException
    {
        #region Public Properties

        public string Type { get; }

        public override string StackTrace { get; }

        public FailureInfo Cause { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public PrestoFailureException(string message, string type, IEnumerable<string> stack, FailureInfo cause) : base(message)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("stack");
            }

            this.Type = type;
            this.StackTrace = String.Join("\n", stack);
            this.Cause = cause ?? throw new ArgumentNullException("cause");
        }

        #endregion
    }
}
