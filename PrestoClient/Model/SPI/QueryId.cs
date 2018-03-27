using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.QueryId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class QueryId
    {
        #region Private Fields

        private static readonly Regex ID_PATTERN = new Regex("[_a-z0-9]+");

        #endregion

        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QueryId(string id)
        {
            if (ValidateId(id))
            {
                this.Id = id;
            }
            else
            {
                throw new ArgumentException("The provided id is invalid.", "id");
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Id;
        }

        public static bool ValidateId(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "The id cannot be null or empty");
            }

            return ID_PATTERN.IsMatch(id);
        }

        #endregion
    }
}
