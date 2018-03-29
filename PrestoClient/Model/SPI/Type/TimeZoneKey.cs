using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.TimeZoneKey.java
    /// </summary>
    [JsonConverter(typeof(TimeZoneKeyConverter))]
    public class TimeZoneKey
    {
        #region Private Fields

        public static readonly TimeZoneKey UTC_KEY = new TimeZoneKey("UTC", 0);

        private static readonly HashSet<TimeZoneKey> TIME_ZONE_KEYS = new HashSet<TimeZoneKey>() {
           UTC_KEY
        };

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
            if (key > TIME_ZONE_KEYS.Count || TIME_ZONE_KEYS.ElementAt(key) == null)
            {
                throw new ArgumentOutOfRangeException("key", $"Invalid time zone key {key}.");
            }

            return TIME_ZONE_KEYS.ElementAt(key);
        }

        #endregion
    }
}
