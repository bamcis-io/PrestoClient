using Newtonsoft.Json;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// from com.facebook.presto.spi.type.NamedTypeSignature.java
    /// </summary>
    public class NamedTypeSignature
    {
        #region Public Properties

        public string Name { get; }

        public TypeSignature TypeSignature { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public NamedTypeSignature(string name, TypeSignature typeSignature)
        {
            this.Name = name;
            this.TypeSignature = typeSignature;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.Name} {this.TypeSignature}";
        }

        #endregion
    }
}
