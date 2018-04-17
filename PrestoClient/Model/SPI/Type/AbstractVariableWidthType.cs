using BAMCIS.PrestoClient.Model.SPI.Block;
using System;

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

        #region Public Methods

        public override IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries, int expectedBytesPerEntry)
        {
            int MaxBlockSizeInBytes;

            if (blockBuilderStatus == null)
            {
                MaxBlockSizeInBytes = BlockBuilderStatus.DEFAULT_MAX_BLOCK_SIZE_IN_BYTES;
            }
            else
            {
                MaxBlockSizeInBytes = blockBuilderStatus.MaxBlockSizeInBytes;
            }

            int ExpectedBytes = Math.Min(expectedEntries * expectedBytesPerEntry, MaxBlockSizeInBytes);

            return new VariableWidthBlockBuilder(
                blockBuilderStatus,
                (expectedBytesPerEntry == 0 ? expectedEntries : Math.Min(expectedEntries, MaxBlockSizeInBytes / expectedBytesPerEntry)),
                ExpectedBytes
                );
        }

        public override IBlockBuilder CreateBlockBuilder(BlockBuilderStatus blockBuilderStatus, int expectedEntries)
        {
            return this.CreateBlockBuilder(blockBuilderStatus, expectedEntries, EXPECTED_BYTES_PER_ENTRY);
        }

        #endregion
    }
}