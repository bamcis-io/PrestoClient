using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.AbstractType.java
    /// </summary>
    public abstract class AbstractType : IType
    {
        #region Protected Fields

        protected readonly TypeSignature Signature;

        protected readonly System.Type JavaType;

        #endregion

        #region Constructors

        public AbstractType(TypeSignature signature, System.Type type)
        {
            this.Signature = signature;
            this.JavaType = type;
        }

        #endregion

        #region Public Methods

        public virtual string GetDisplayName()
        {
            return this.Signature.ToString();
        }

        public System.Type GetJavaType()
        {
            return this.JavaType;
        }

        public virtual IEnumerable<IType> GetTypeParameter()
        {
            return new IType[0];
        }

        public TypeSignature GetTypeSignature()
        {
            return this.Signature;
        }

        public virtual bool IsComparable()
        {
            return false;
        }

        public virtual bool IsOrderable()
        {
            return false;
        }

        public override string ToString()
        {
            return this.Signature.ToString();
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

            return this.Signature.Equals(((IType)obj).GetTypeSignature());
        }

        public override int GetHashCode()
        {
            return this.Signature.GetHashCode();
        }

        #endregion
    }
}
