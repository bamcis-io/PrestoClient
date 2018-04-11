using System;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.VarcharType.java
    /// </summary>
    public sealed class VarcharType : AbstractVariableWidthType
    {
        #region Private Fields

        private int _Length;

        #endregion

        #region Public Properties

        public static readonly int UNBOUNDED_LENGTH = Int32.MaxValue;
        public static readonly int MAX_LENGTH = Int32.MaxValue - 1;
        public static readonly VarcharType VARCHAR = new VarcharType(UNBOUNDED_LENGTH);

        public int Length {
            get
            {
                if (this.IsUnbounded())
                {
                    throw new InvalidOperationException("Cannot get size of unbounded VARCHAR.");
                }

                return this._Length;
            }
        }

        #endregion

        #region Constructors

        private VarcharType(int length) : base(new TypeSignature(StandardTypes.VARCHAR, new TypeSignatureParameter((long)length)), typeof(string))
        {
            ParameterCheck.OutOfRange(length >= 0, "length");

            this._Length = length;
        }

        #endregion

        #region Public Methods

        public bool IsUnbounded()
        {
            return this._Length == UNBOUNDED_LENGTH;
        }

        public override bool IsComparable()
        {
            return true;
        }

        public override bool IsOrderable()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            VarcharType other = (VarcharType)obj;

            return Object.Equals(this._Length, other._Length);
        }

        public override int GetHashCode()
        {
            return this._Length.GetHashCode();
        }

        public string DisplayName()
        {
            if (this._Length == UNBOUNDED_LENGTH)
            {
                return this.Signature.Base;
            }

            return this.Signature.ToString();
        }

        public override string ToString()
        {
            return DisplayName();
        }

        #endregion
    }
}
