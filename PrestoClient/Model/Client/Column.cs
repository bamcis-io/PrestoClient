using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.Column.java
    /// </summary>
    public class Column
    {
        #region Public Properties

        public string Name { get; }

        public string Type { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ClientTypeSignature TypeSignature { get; }

        #endregion

        #region Constructors

        public Column(string name, string type, ClientTypeSignature typeSignature)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (String.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type");
            }

            this.Name = name;
            this.Type = type;
            this.TypeSignature = typeSignature;
        }

        #endregion
    }
}