namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.AllOrNone.java
    /// </summary>
    public interface IAllOrNone
    {
        /// <summary>
        /// Return true if all values are permitted, false if no values are permitted
        /// </summary>
        /// <returns></returns>
        bool IsAll();
    }
}
