using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// Represents a set of options that are specific to a single query
    /// </summary>
    public class QueryOptions
    {
        #region Private Fields

        private HashSet<string> _ClientTags;
        private IDictionary<string, string> _Properties;

        #endregion

        #region Public Properties

        /// <summary>
        /// Properties to add for this query that are used by the Presto 
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
        /// Client provided tags that are associated with the query
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
        /// A custom transactionId provided by the client
        /// </summary>
        public string TransactionId { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a new query options with default values
        /// </summary>
        public QueryOptions()
        {
            this.Properties = new Dictionary<string, string>();
            this.PreparedStatements = new Dictionary<string, string>();
            this.ClientTags = new HashSet<string>();
            this.TransactionId = String.Empty;
        }

        #endregion
    }
}