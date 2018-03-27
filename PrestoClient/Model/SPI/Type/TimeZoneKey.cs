using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.TimeZoneKey.java
    /// </summary>
    [JsonConverter(typeof(TimeZoneKeyConverter))]
    public class TimeZoneKey
    {
        #region Private Fields

        private static readonly TimeZoneKey[] TIME_ZONE_KEYS = { };

        #endregion

        #region Public Properties

        public string Id { get; }

        public short Key { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TimeZoneKey(string id, short key)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id", "The id cannot be null or empty.");
            }

            if (key < 0)
            {
                throw new ArgumentOutOfRangeException("key", "The key cannot be less than zero.");
            }

            this.Id = id;
            this.Key = key;
        }

        #endregion

        #region Public Methods

        public static TimeZoneKey GetTimeZoneKey(short key)
        {
            if (key > TIME_ZONE_KEYS.Length || TIME_ZONE_KEYS[key] == null)
            {
                throw new ArgumentOutOfRangeException("key", $"Invalid time zone key {key}.");
            }

            return TIME_ZONE_KEYS[key];
        }

        #endregion
    }
}
