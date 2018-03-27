using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Transaction
{
    /// <summary>
    /// From com.facebook.transaction.TransactionId.java
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class TransactionId
    {
        #region Public Properties

        public Guid UUID { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public TransactionId(Guid uuid)
        {
            this.UUID = uuid;
        }

        public TransactionId(string uuid)
        {
            this.UUID = Guid.Parse(uuid);
        }

        #endregion

        #region Public Methods

        public static TransactionId Create()
        {
            return new TransactionId(Guid.NewGuid());
        }

        public static TransactionId ValueOf(string value)
        {
            return new TransactionId(Guid.Parse(value));
        }

        public override string ToString()
        {
            return this.UUID.ToString();
        }

        #endregion
    }
}
