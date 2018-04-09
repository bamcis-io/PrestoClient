namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.Marker.java (internal enum Bound)
    /// </summary>
    public enum Bound
    {
        /// <summary>
        /// Lower than the value, but infinitesimally close to the value
        /// </summary>
        BELOW,

        /// <summary>
        /// Exactly the value
        /// </summary>
        EXACTLY,

        /// <summary>
        /// Higher than the value, but infinitesimally close to the value
        /// </summary>
        ABOVE
    }
}
