using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Model.SPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Execution
{
    /// <summary>
    /// From com.facebook.presto.execution.ExecutionFailureInfo.java
    /// </summary>
    public class ExecutionFailureInfo
    {
        #region Public Properties

        public string Type { get; }

        public string Message { get; }

        public ExecutionFailureInfo Cause { get; }

        public IEnumerable<ExecutionFailureInfo> Suppressed { get; }

        public IEnumerable<string> Stack { get; }

        public ErrorLocation ErrorLocation { get; }

        public ErrorCode ErrorCode { get; }

        public HostAddress RemoteHost { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ExecutionFailureInfo(
            string type,
            string message,
            ExecutionFailureInfo cause,
            IEnumerable<ExecutionFailureInfo> suppressed,
            IEnumerable<string> stack,
            ErrorLocation errorLocation,
            ErrorCode errorCode,
            HostAddress remoteHost
        )
        {
            if (String.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type", "The type cannot be null or empty.");
            }

            this.Type = type;
            this.Message = message;
            this.Cause = cause;
            this.Suppressed = suppressed ?? throw new ArgumentNullException("suppressed", "Suppressed cannot be null.");
            this.Stack = stack ?? throw new ArgumentNullException("stack", "Stack cannot be null.");
            this.ErrorLocation = errorLocation;
            this.ErrorCode = errorCode;
            this.RemoteHost = remoteHost;
        }

        #endregion
    }
}
