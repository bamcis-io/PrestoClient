using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From io.airlift.units.Duration.java
    /// </summary>
    [JsonConverter(typeof(TimeSpanConverter))]
    public class Duration
    {

    }
}
