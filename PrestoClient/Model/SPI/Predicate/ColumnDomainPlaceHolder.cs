namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// This is a generic placeholder class for ColumnDomain<T> just used 
    /// to make serialization simpler and not needing to code all of the implementers
    /// of the TupleDomain
    /// </summary>
    public class ColumnDomainPlaceHolder
    {
        public dynamic Column { get; set; }

        public dynamic Domain { get; set; }
    }
}
