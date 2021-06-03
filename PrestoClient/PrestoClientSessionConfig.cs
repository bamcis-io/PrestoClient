using BAMCIS.PrestoClient.Model;
using BAMCIS.PrestoClient.Model.SPI.Type;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BAMCIS.PrestoClient
{
    /// <summary>
    /// The set of options that can be supplied to the PrestoClient to configure
    /// how the client connects to the coordinator and what catalog and schema to use.
    /// 
    /// There are several options such User and Properties that will apply to all
    /// queries made using the config passed to the PrestoClient. Items like Properties
    /// can also be specified in QueryOptions, which will add their values to the values
    /// provided here, not overwrite them.
    /// </summary>
    public sealed class PrestoClientSessionConfig
    {
        #region Defaults

        private static readonly int _QUERY_STATE_CHECK_INTERVAL = 800; // Milliseconds
        private static readonly string _DEFAULT_HOST = "localhost";
        private static readonly int _DEFAULT_PORT = 8080;
        private static readonly long _DEFAULT_TIMEOUT = -1; // Anything 0 or below indicates that the client will never timeout a query

        #endregion

        #region Private Fields

        private string _User;
        private int _Port;
        private int _CheckInterval;
        private HashSet<string> _ClientTags;
        private IDictionary<string, string> _Properties;
        private IDictionary<string, string> _Headers;

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
        /// The password to use in HTTP basic auth with the presto server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The catalog to use for interaction with presto.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// The schema to connect to in presto. This defaults to 'default'.
        /// </summary>
        public string Schema { get; set; }

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

        /// <summary>
        /// Extra info about the client making the query
        /// </summary>
        public string ClientInfo { get; set; }

        /// <summary>
        /// This will always default to "0" representing UTC
        /// </summary>
        public TimeZoneKey TimeZone { get; }

        /// <summary>
        /// Defaults to CurrentCulture
        /// </summary>
        public CultureInfo Locale { get; set; }

        /// <summary>
        /// Properties to add to the session that are used by the Presto 
        /// engine or connectors to customize the query execution, for example:
        /// 
        /// { "hive.optimized_reader_enabled", "true" }
        /// 
        /// or
        /// 
        /// { "optimize_hash_generation", "true" }
        /// 
        /// See https://prestodb.io/docs/current/admin/properties.html for information on
        /// available properties that can be set
        /// </summary>
        public IDictionary<string, string> Properties
        {

            get
            {
                return this._Properties;
            }
            set
            {
                foreach (KeyValuePair<string, string> Item in value)
                {
                    if (String.IsNullOrEmpty(Item.Key))
                    {
                        throw new ArgumentNullException("Properties", "Session property key name is empty.");
                    }

                    if (Item.Key.Contains("="))
                    {
                        throw new FormatException($"Session property name must not contain '=' : {Item.Key}");
                    }

                    string AsciiKey = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Key));

                    if (!AsciiKey.Equals(Item.Key))
                    {
                        throw new FormatException($"Session property name contains non US_ASCII characters: {Item.Key}");
                    }

                    string AsciiValue = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Value));

                    if (!AsciiValue.Equals(Item.Value))
                    {
                        throw new FormatException($"Session property value contains non US_ASCII characters: {Item.Value}");
                    }
                }

                this._Properties = value;
            }
        }

        /// <summary>
        /// Headers to be appended to each request made to presto db.
        /// For example: { "X-Trino-Catalog", "hive" }
        /// </summary>
        public IDictionary<string, string> Headers
        {

            get
            {
                return this._Headers;
            }
            set
            {
                foreach (KeyValuePair<string, string> Item in value)
                {
                    if (String.IsNullOrEmpty(Item.Key))
                    {
                        throw new ArgumentNullException("Headers", "Header key name is empty.");
                    }

                    if (Item.Key.Contains("="))
                    {
                        throw new FormatException($"Header key name must not contain '=' : {Item.Key}");
                    }

                    string AsciiKey = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Key));

                    if (!AsciiKey.Equals(Item.Key))
                    {
                        throw new FormatException($"Header key name contains non US_ASCII characters: {Item.Key}");
                    }

                    string AsciiValue = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Item.Value));

                    if (!AsciiValue.Equals(Item.Value))
                    {
                        throw new FormatException($"Header value contains non US_ASCII characters: {Item.Value}");
                    }
                }

                this._Headers = value;
            }
        }

        /// <summary>
        /// A set of prepared statements. The key value is the statement
        /// name and the value is the statement, for example:
        /// 
        /// { "my_select_1", "SELECT * FROM nation WHERE regionKey = ? and nationKey > ?"}
        /// 
        /// This could then be executed with EXECUTE my_select_1 USING 1, 3;
        /// 
        /// 1 is provided the '?' to compare against regionKey and 3 is provided to '?' to compare
        /// against nationKey.
        /// </summary>
        public IDictionary<string, string> PreparedStatements { get; set; }

        /// <summary>
        /// Client provided tags that are associated with the session
        /// and are displayed in the SessionRepresentation
        /// </summary>
        public HashSet<string> ClientTags
        {

            get
            {
                return this._ClientTags;
            }
            set
            {
                foreach (string Tag in value)
                {
                    if (Tag.Contains(","))
                    {
                        throw new ArgumentException($"The client tag {Tag} cannot contain a comma ','.");
                    }
                }

                this._ClientTags = value;
            }
        }

        /// <summary>
        /// Enables debug information
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// The timeout in seconds for how long to wait for a query to finish
        /// </summary>
        public long ClientRequestTimeout { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new client options set with default values.
        /// </summary>
        public PrestoClientSessionConfig()
        {
            this.Host = _DEFAULT_HOST;
            this.Port = _DEFAULT_PORT;
            this.User = Environment.GetEnvironmentVariable("USERNAME") ?? Environment.GetEnvironmentVariable("USER");
            this.CheckInterval = _QUERY_STATE_CHECK_INTERVAL;
            this.IgnoreSslErrors = false;
            this.UseSsl = false;
            this.Version = PrestoApiVersion.V1;
            this.ClientTags = new HashSet<string>();
            this.Debug = false;
            this.Properties = new Dictionary<string, string>();
            this.PreparedStatements = new Dictionary<string, string>();
            this.TimeZone = TimeZoneKey.GetTimeZoneKey(0);
            this.Locale = CultureInfo.CurrentCulture;
            this.ClientRequestTimeout = _DEFAULT_TIMEOUT;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        public PrestoClientSessionConfig(string host, int port, string catalog) : this()
        {
            this.Host = host;
            this.Port = port;
            this.Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        public PrestoClientSessionConfig(string catalog) : this()
        {
            this.Catalog = catalog;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientSessionConfig(string catalog, string schema) : this(catalog)
        {
            this.Schema = schema;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientSessionConfig(string host, string catalog, string schema) : this(catalog, schema)
        {
            this.Host = host;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        public PrestoClientSessionConfig(string host, int port, string catalog, string schema) : this(host, port, catalog)
        {
            this.Schema = schema;
        }

        /// <summary>
        /// Creates a new client options set with the specified properties.
        /// </summary>
        /// <param name="host">The IP or DNS host name of the presto coordinator</param>
        /// <param name="port">The web interface port</param>
        /// <param name="catalog">The default catalog to use</param>
        /// <param name="schema">The schema to use</param>
        /// <param name="user">The user name associated with this session</param>
        /// <param name="clientTags">Client tags to apply to all requests in this session</param>
        /// <param name="clientInfo">Additional info about the client</param>
        /// <param name="locale">The non-default locale to use for language settings</param>
        /// <param name="properties">Persistent session properties to set</param>
        /// <param name="preparedStatements">Prepared statements that will be available to all queries made in this session</param>
        /// <param name="debug">Whether to enable debug logging for this session</param>
        /// <param name="timeout">The timeout in seconds for queries in this session</param>
        public PrestoClientSessionConfig(
            string host,
            int port,
            string catalog,
            string schema,
            string user,
            HashSet<string> clientTags,
            string clientInfo,
            CultureInfo locale,
            IDictionary<string, string> properties,
            IDictionary<string, string> preparedStatements,
            bool debug,
            long timeout
            ) : this(host, port, catalog, schema)
        {
            this.User = user;
            this.ClientTags = clientTags;
            this.ClientInfo = clientInfo;
            this.Locale = locale;
            this.Properties = properties;
            this.PreparedStatements = preparedStatements;
            this.Debug = debug;
            this.ClientRequestTimeout = timeout;
        }

        #endregion
    }
}