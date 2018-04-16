using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.TupleDomain.java (internal class ColumnDomain)
    /// 
    /// Used for serialization only
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(ColumnDomainConverter<>))]
    public class ColumnDomain<T>
    {
        #region Public Properties

        public T Column { get; }

        public Domain Domain { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ColumnDomain(T column, Domain domain)
        {
            this.Column = column;
            this.Domain = domain ?? throw new ArgumentNullException("domain");
        }

        #endregion
    }
}
