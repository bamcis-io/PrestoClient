using System;

namespace BAMCIS.PrestoClient
{
    /// <summary>
    /// Headers uses in Trino requests and responses
    /// </summary>
    public class ProtocolHeaders
    {
        #region Private Fields

        private string prefix;

        #endregion

        #region Public Fields

        /// <summary>
        /// The name added to the header prefix, for example, if this is
        /// 'Trino', then the header prefix is 'X-Trino-'. Specify 'Presto'
        /// for legacy support.
        /// </summary>
        public string Name { get; }

        public string REQUEST_USER => $"{this.prefix}User";
        public string REQUEST_SOURCE => $"{this.prefix}Source";
        public string REQUEST_CATALOG => $"{this.prefix}Catalog";
        public string REQUEST_SCHEMA => $"{this.prefix}Schema";
        public string REQUEST_PATH => $"{this.prefix}Path";
        public string REQUEST_TIME_ZONE => $"{this.prefix}Time-Zone";
        public string REQUEST_LANGUAGE => $"{this.prefix}Language";
        public string REQUEST_TRACE_TOKEN => $"{this.prefix}Trace-Token";
        public string REQUEST_SESSION => $"{this.prefix}Session";
        public string REQUEST_ROLE => $"{this.prefix}Role";
        public string REQUEST_PREPARED_STATEMENT => $"{this.prefix}Prepared-Statement";
        public string REQUEST_TRANSACTION_ID => $"{this.prefix}Transaction-Id";
        public string REQUEST_CLIENT_INFO => $"{this.prefix}Client-Info";
        public string REQUEST_CLIENT_TAGS => $"{this.prefix}Client-Tags";
        public string REQUEST_CLIENT_CAPABILITIES => $"{this.prefix}Client-Capabilities";
        public string REQUEST_RESOURCE_ESTIMATE => $"{this.prefix}Resource-Estimate";
        public string REQUEST_EXTRA_CREDENTIAL => $"{this.prefix}Extra-Credential";
        public string RESPONSE_SET_CATALOG => $"{this.prefix}Set-Catalog";
        public string RESPONSE_SET_SCHEMA => $"{this.prefix}Set-Schema";
        public string RESPONSE_SET_PATH => $"{this.prefix}Set-Path";
        public string RESPONSE_SET_SESSION => $"{this.prefix}Set-Session";
        public string RESPONSE_CLEAR_SESSION => $"{this.prefix}Clear-Session";
        public string RESPONSE_SET_ROLE => $"{this.prefix}Set-Role";
        public string RESPONSE_ADDED_PREPARE => $"{this.prefix}Added-Prepare";
        public string RESPONSE_DEALLOCATED_PREPARE => $"{this.prefix}Deallocated-Prepare";
        public string RESPONSE_STARTED_TRANSACTION_ID => $"{this.prefix}Started-Transaction-Id";
        public string RESPONSE_CLEAR_TRANSACTION_ID => $"{this.prefix}Clear-Transaction-Id";
        public string RESPONSE_DATA_NEXT_URI => $"{this.prefix}Data-Next-Uri";

        #endregion

        public static ProtocolHeaders TRINO_HEADERS = new ProtocolHeaders("Trino");

        #region Constructors

        public ProtocolHeaders(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "The name cannot be null or empty.");
            }

            this.Name = name;
            this.prefix = $"X-{this.Name}-";
        }

        #endregion
    }
}
