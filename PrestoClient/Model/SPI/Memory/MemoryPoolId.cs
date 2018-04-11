using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI.Memory
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class MemoryPoolId
    {
        #region Public Properties

        public string Id { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public MemoryPoolId(string id)
        {
            ParameterCheck.NotNullOrEmpty(id, "id");

            this.Id = id;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Id;
        }

        #endregion
    }
}
