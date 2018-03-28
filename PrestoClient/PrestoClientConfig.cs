using BAMCIS.PrestoClient.Model;
using BAMCIS.PrestoClient.Model.SPI.Type;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BAMCIS.PrestoClient
{
    /// <summary>
    /// The set of options that can be supplied to the PrestoClient to configure
    /// how the client connects to the coordinator and what catalog and schema to use.
    /// </summary>
    public sealed class PrestoClientConfig
    {
        #region Defaults

        private static readonly int _QUERY_STATE_CHECK_INTERVAL = 800; // Milliseconds
        private static readonly string _DEFAULT_HOST = "localhost";
        private static readonly int _DEFAULT_PORT = 8080;

        #endregion

        #region Private Fields

        private string _Catalog;
        private string _Schema;
        private string _Host;
        private string _User;
        private int _Port;
        private int _CheckInterval;

        #endregion

        #region Public Properties

        /// <summary>
        /// The DNS host name or IP address of the presto coordinator.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The web interface port of the presto coordinator, usually 8080.
        /// </summary>
        public int Port
        {
            get
            {
                return this._Port;
            }
            set
            {
                if (value <= 65535 && value >= 1)
                {
                    this._Port = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Port", "The port must be between 1 and 65535.");
                }
            }
        }

        /// <summary>
        /// The name of the user connecting to presto. This defaults to the current
        /// user.
        /// </summary>
        public string User
        {
            get
            {
                return this._User;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("User", "The user name cannot be null or empty.");
                }

                this._User = value;
            }
        }

        /// <summary>
        /// The catalog to use for interaction with presto.
        /// </summary>
        public string Catalog
        {
            get
            {
                return this._Catalog;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Catalog", "The catalog cannot be null or empty.");
                }

                this._Catalog = value;
            }
        }

        /// <summary>
        /// The schema to connect to in presto. This defaults to 'default'.
        /// </summary>
        public string Schema
        {
            get
            {
                return this._Schema;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Schema", "The schema cannot be null or empty.");
                }

                this._Schema = value;
            }
        }

        /// <summary>
        /// The amount of time in milliseconds that the client will wait in between
        /// checks for new data from presto. The minimum interval is 50ms and the maximum interval
        /// is 5000ms. This defaults to 800ms.
        /// </summary>
        public int CheckInterval
        {
            get
            {
                return this._CheckInterval;
            }
            set
            {
                if (value < 50)
                {
                    throw new ArgumentOutOfRangeException("CheckInterval", "The minimum check interval is 50ms.");
                }

                if (value > 5000)
                {
                    throw new ArgumentOutOfRangeException("CheckInterval", "The maximum check interval is 5000ms.");
                }

                this._CheckInterval = value;
            }

        }

        /// <summary>
        /// Whether to ignore SSL errors produced by connecting to presto over an SSL
        /// connection that may be using expired or untrusted certificates.
        /// </summary>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Specifies that the connections made to the presto coordinator and possibly
        /// worker nodes should use SSL. 
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// The version of the REST API being used, this defaults to V1.
        /// </summary>
        public PrestoApiVersion Version { get; }

        public string Source { get; set; }

        public HashSet<string> ClientTags { get; set; }

        public string ClientInfo { get; set; }

        public TimeZoneKey TimeZone { get; set; }

        public CultureInfo Locale { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public IDictionary<string, string> PreparedStatements { get; set; }

        public bool Debug { get; set; }

        /// <summary>
        /// The timeout in seconds for how long to wait for a query to finish
        /// </summary>
        public int ClientRequestTimeout { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new client options set with default values.
        /// </summary>
        public PrestoClientConfig()
        {
            this.Host = _DEFAULT_HOST;
            this.Port = _DEFAULT_PORT;
            this.User = Environment.GetEnvironmentVariable("USERNAME") ?? Environment.GetEnvironmentVariable("USER");
            this.Schema = "default";
            this.CheckInterval = _QUERY_STATE_CHECK_INTERVAL;
            this.IgnoreSslErrors = false;
            this.UseSsl = false;
            this.Version = PrestoApiVersion.V1;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        public PrestoClientConfig(string host, int port, string catalog) : this()
        {
            this.Host = host;
            this.Port = port;
            this.Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        public PrestoClientConfig(string catalog) : this()
        {
            this.Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientConfig(string catalog, string schema) : this(catalog)
        {
            this.Schema = schema;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientConfig(string host, string catalog, string schema) : this(catalog, schema)
        {
            this.Host = host;
        }

        /// <summary>
        ///  Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">>The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientConfig(string host, int port, string catalog, string schema) : this(host, port, catalog)
        {
            this.Schema = schema;
        }

        #endregion
    }
}