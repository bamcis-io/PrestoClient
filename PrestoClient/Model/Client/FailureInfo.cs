using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.FailureInfo.java
    /// </summary>
    public class FailureInfo
    {
        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; }

        public string Message { get; }

        public FailureInfo Cause { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<FailureInfo> Suppressed { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Stack { get; }

        public ErrorLocation ErrorLocation { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public FailureInfo(
            string type, 
            string message, 
            FailureInfo cause, 
            IEnumerable<FailureInfo> suppressed, 
            IEnumerable<string> stack, 
            ErrorLocation errorLocation
            )
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            this.Type = type;
            this.Message = message;
            this.Cause = cause;
            this.Suppressed = suppressed ?? throw new ArgumentNullException("suppressed");
            this.Stack = stack;
            this.ErrorLocation = errorLocation;
        }

        #endregion

        #region Public Methods

        public PrestoException ToException()
        {
            return ToException(this);
        }

        public PrestoFailureException ToPrestoFailureException()
        {
            return ToException(this);
        }

        private static PrestoFailureException ToException(FailureInfo failureInfo)
        {
            if (failureInfo == null)
            {
                return null;
            }

            return new PrestoFailureException(
                failureInfo.Message,
                failureInfo.Type,
                failureInfo.Stack,
                failureInfo.Cause
            );
        }

        #endregion
    }
}