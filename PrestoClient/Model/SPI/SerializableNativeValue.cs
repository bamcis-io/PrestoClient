using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// TODO: In progress for TupleDomain
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
