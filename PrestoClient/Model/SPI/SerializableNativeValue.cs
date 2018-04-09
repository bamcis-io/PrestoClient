using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    public sealed class SerializableNativeValue<T>
    {
        #region Public Properties

        public System.Type Type { get; }

        public IComparable<T> Value { get; }

        #endregion

        #region Constructors

        public SerializableNativeValue(System.Type type, IComparable<T> value)
        {
            this.Type = type;
            this.Value = value;
        }

        #endregion
    }
}
