namespace BAMCIS.PrestoClient.Model.SPI.Connector
{
    /// <summary>
    /// From com.facebook.presto.spi.connector.ConnectorPartitioningHandle.java
    /// </summary>
    public interface IConnectorPartitioningHandle
    {
        bool IsSingleNode();

        bool IsCoordinatorOnly();
    }
}
