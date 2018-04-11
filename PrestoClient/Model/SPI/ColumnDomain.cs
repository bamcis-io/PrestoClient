using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.TupleDomain.java (internal class ColumnDomain)
    /// 
    /// Used for serialization only
    /// 
    /// TODO: In progress for TupleDomain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnDomain<T>
    {
        #region Public Properties

        public IColumnHandle ColumnHandle { get; }

        public Domain<T> Domain { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ColumnDomain(IColumnHandle columnHandle, Domain<T> domain)
        {
            this.ColumnHandle = columnHandle ?? throw new ArgumentNullException("columnHandle");
            this.Domain = domain ?? throw new ArgumentNullException("domain");
        }

        #endregion
    }
}
