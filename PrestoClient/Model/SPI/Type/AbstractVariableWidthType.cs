namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.spi.type.AbstractVariableWidthType.java
    /// </summary>
    public abstract class AbstractVariableWidthType : AbstractType, IVariableWidthType 
    {
        #region Private Fields

        private static readonly int EXPECTED_BYTES_PER_ENTRY = 32;


        #endregion

        #region Constructors

        protected AbstractVariableWidthType(TypeSignature signature, System.Type type) : base(signature, type)
        {
        }

        #endregion
    }
}
