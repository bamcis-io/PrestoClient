using BAMCIS.PrestoClient.Model.SPI.Block;
using System;
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
            return Hashing.Hash(this.Signature);
        }

        public long Hash(IBlock block, int position)
        {
            throw new InvalidOperationException($"{this.GetTypeSignature()} type is not comparable.");
        }

        public int CompareTo(IBlock leftBlock, int leftPosition, IBlock rightBlock, int rightPosition)
        {
            throw new InvalidOperationException($"{this.GetTypeSignature()} type is not orderable.");
        }

        public bool EqualTo(IBlock leftBlock, int leftPosition, IBlock rightBlock, int rightPosition)
        {
            throw new InvalidOperationException($"{this.GetTypeSignature()} type is not comparable.");
        }

        public bool GetBoolean(IBlock block, int position)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteBoolean(IBlockBuilder blockBuilder, bool value)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public long GetLong(IBlock block, int position)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteLong(IBlockBuilder blockBuilder, long value)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public double GetDouble(IBlock block, int position)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteDouble(IBlockBuilder blockBuilder, double value)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public Slice GetSlice(IBlock block, int position)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteSlice(IBlockBuilder blockBuilder, Slice value)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteSlice(IBlockBuilder blockBuilder, Slice value, int offset, int length)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public object GetObject(IBlock block, int position)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        public void WriteObject(IBlockBuilder blockBuilder, object value)
        {
            throw new InvalidOperationException(this.GetType().Name);
        }

        #endregion

        #region Not Implemented Interface Methods

        public void AppendTo(IBlock block, int position, IBlockBuilder blockBuilder)
        {
            throw new NotImplementedException();
        }

        public IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries)
        {
            return this.CreateBlockBuilder(blockBuilderStatus, expectedEntries, 0);
        }

        public IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries, int expectedBytesPerEntry)
        {
            throw new NotImplementedException();
        }

        public object GetObjectValue(IConnectorSession session, IBlock block, int position)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
