using BAMCIS.PrestoClient.Model.SPI.Security;
using BAMCIS.PrestoClient.Model.SPI.Type;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System.Globalization;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ConnectorSession.java
    /// </summary>
    [JsonConverter(typeof(DynamicInterfaceConverter))]
    public interface IConnectorSession
    {
        string GetQueryId();

        string GetSource();

        string GetUser();

        Identity GetIdentity();

        TimeZoneKey GetTimeZoneKey();

        CultureInfo GetLocale();

        long GetStartTime();

        object GetProperty(string name, System.Type type);
    }
}
