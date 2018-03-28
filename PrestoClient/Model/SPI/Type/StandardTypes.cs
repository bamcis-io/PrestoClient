namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.StandardTypes.java
    /// </summary>
    public sealed class StandardTypes
    {
        #region Public Properties

        public static readonly string BIGINT = "bigint";
        public static readonly string INTEGER = "integer";
        public static readonly string SMALLINT = "smallint";
        public static readonly string TINYINT = "tinyint";
        public static readonly string BOOLEAN = "boolean";
        public static readonly string DATE = "date";
        public static readonly string DECIMAL = "decimal";
        public static readonly string REAL = "real";
        public static readonly string DOUBLE = "double";
        public static readonly string HYPER_LOG_LOG = "HyperLogLog";
        public static readonly string P4_HYPER_LOG_LOG = "P4HyperLogLog";
        public static readonly string INTERVAL_DAY_TO_SECOND = "interval day to second";
        public static readonly string INTERVAL_YEAR_TO_MONTH = "interval year to month";
        public static readonly string TIMESTAMP = "timestamp";
        public static readonly string TIMESTAMP_WITH_TIME_ZONE = "timestamp with time zone";
        public static readonly string TIME = "time";
        public static readonly string TIME_WITH_TIME_ZONE = "time with time zone";
        public static readonly string VARBINARY = "varbinary";
        public static readonly string VARCHAR = "varchar";
        public static readonly string CHAR = "char";
        public static readonly string ROW = "row";
        public static readonly string ARRAY = "array";
        public static readonly string MAP = "map";
        public static readonly string JSON = "json";
        public static readonly string IPADDRESS = "ipaddress";
        public static readonly string GEOMETRY = "Geometry";
        public static readonly string BING_TILE = "BingTile";

        #endregion

        #region Constructors

        private StandardTypes()
        { }

        #endregion
    }
}
