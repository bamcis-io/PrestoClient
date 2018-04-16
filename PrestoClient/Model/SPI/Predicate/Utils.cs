using BAMCIS.PrestoClient.Model.SPI.Block;
using BAMCIS.PrestoClient.Model.SPI.Type;
using System;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Utils.java
    /// </summary>
    public sealed class Utils
    {
        #region Constructors

        private Utils()
        { }

        #endregion

        #region Public Static Methods

        public static IBlock NativeValueToBlock(IType type, object obj)
        {
            if (obj != null && obj.GetType() != type.GetJavaType())
            {
                throw new ArgumentException($"Object {obj.ToString()} does not match type {type.GetJavaType()}.");
            }

            IBlockBuilder BlockBuilder = type.CreateBlockBuilder(new BlockBuilderStatus(), 1);
            TypeUtils.WriteNativeValue(type, BlockBuilder, obj);

            return BlockBuilder.Build();
        }

        public static object BlockToNativeValue(IType type, IBlock block)
        {
            return TypeUtils.ReadNativeValue(type, block, 0);
        }

        #endregion
    }
}
