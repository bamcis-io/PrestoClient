using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.DiscreteValues.java
    /// </summary>
    public interface IDiscreteValues
    {
        /// <summary>
        /// Return true if the values are to be included, false if the values are to be excluded
        /// </summary>
        /// <returns></returns>
        bool IsWhiteList();

        IEnumerable<object> GetValues();
    }
}
