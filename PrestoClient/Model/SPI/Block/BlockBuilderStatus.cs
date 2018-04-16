using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BAMCIS.PrestoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.BlockBuilderStatus.java
    /// </summary>
    public class BlockBuilderStatus
    {
        #region Private Fields

        private PageBuilderStatus _PageBuilderStatus;

        private int _CurrentSize;

        #endregion

        #region Public Properties

        public int MaxBlockSizeInBytes { get; }

        #endregion

        #region Public Fields

        public static readonly int INSTANCE_SIZE = DeepInstanceSize(typeof(BlockBuilderStatus));

        public static readonly int DEFAULT_MAX_BLOCK_SIZE_IN_BYTES = 64 * 1024;

        #endregion

        #region Constructors

        public BlockBuilderStatus() : this(new PageBuilderStatus(PageBuilderStatus.DEFAULT_MAX_PAGE_SIZE_IN_BYTES, DEFAULT_MAX_BLOCK_SIZE_IN_BYTES), DEFAULT_MAX_BLOCK_SIZE_IN_BYTES)
        {
        }

        public BlockBuilderStatus(PageBuilderStatus pageBuilderStatus, int maxBlockSizeInBytes)
        {
            this._PageBuilderStatus = pageBuilderStatus ?? throw new ArgumentNullException("pageBuilderStatus");
            this.MaxBlockSizeInBytes = maxBlockSizeInBytes;
        }

        #endregion

        #region Private Methods

        private static int DeepInstanceSize(System.Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsArray)
            {
                throw new ArgumentException($"Cannot determine size of {type.Name} because it contains an array.");
            }

            if (type.IsInterface)
            {
                throw new ArgumentException($"{type.Name} is an interface.");
            }

            if (type.IsAbstract)
            {
                throw new ArgumentException($"{type.Name} is abstract.");
            }

            if (!type.BaseType.Equals(typeof(object)))
            {
                throw new ArgumentException($"Cannot determine size of a subclass. {type.Name} extends from {type.BaseType.Name}.");
            }

            int Size = Marshal.SizeOf(type);

            foreach (PropertyInfo Info in type.GetProperties())
            {
                if (!Info.PropertyType.IsPrimitive)
                {
                    Size += DeepInstanceSize(Info.PropertyType);
                }
            }

            return Size;
        }

        #endregion

        #region Public Methods

        public void AddBytes(int bytes)
        {
            this._CurrentSize += bytes;
            this._PageBuilderStatus.AddBytes(bytes);

            if (this._CurrentSize >= this.MaxBlockSizeInBytes)
            {
                this._PageBuilderStatus.Full = true;
            }
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxSizeInBytes", this.MaxBlockSizeInBytes)
                .Add("currentSize", this._CurrentSize)
                .ToString();
        }

        #endregion
    }
}
