using BAMCIS.PrestoClient.Model.SPI.Block;
using System;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.TypeUtils.java
    /// </summary>
    public sealed class TypeUtils
    {
        #region Public Fields

        public static readonly int NULL_HASH_CODE = 0;

        #endregion

        #region Constructors

        private TypeUtils()
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the native value as an object in the value at {@code position} of {@code block}.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="block"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static object ReadNativeValue(IType type, IBlock block, int position)
        {
            System.Type JavaType = type.GetJavaType();

            if (block.IsNull(position))
            {
                return null;
            }

            if (JavaType == typeof(long))
            {
                return type.GetLong(block, position);
            }

            if (JavaType == typeof(double))
            {
                return type.GetDouble(block, position);
            }

            if (JavaType == typeof(bool))
            {
                return type.GetBoolean(block, position);
            }

            if (JavaType == typeof(Slice))
            {
                return type.GetSlice(block, position);
            }

            return type.GetObject(block, position);
        }

        /// <summary>
        /// Write a native value object to the current entry of {@code blockBuilder}.
        /// 
        /// TODO: Add in slice logic
        /// </summary>
        /// <param name="type"></param>
        /// <param name="blockBuilder"></param>
        /// <param name="value"></param>
        public static void WriteNativeValue(IType type, IBlockBuilder blockBuilder, object value)
        {
            if (value == null)
            {
                blockBuilder.AppendNull();
            }
            else if (type.GetJavaType() == typeof(bool))
            {
                type.WriteBoolean(blockBuilder, (bool)value);
            }
            else if (type.GetJavaType() == typeof(double))
            {
                type.WriteDouble(blockBuilder, (double)value);
            }
            else if (type.GetJavaType() == typeof(long))
            {
                type.WriteLong(blockBuilder, (long)value);
            }
            else if (type.GetJavaType() == typeof(Slice))
            {
                Slice Slice = (Slice)value;

                if (value is byte[])
                {
                    //Slice = Slices.WrappedBuffer((byte[]) value);
                }
                else if (value is string)
                {
                    //Slice = Slices.Utf8Slice((string)value);
                }
                else
                {
                    Slice = (Slice)value;
                }

                type.WriteSlice(blockBuilder, Slice, 0, Slice.Size);
            }
            else
            {
                type.WriteObject(blockBuilder, value);
            }
        }

        public static long HashPosition(IType type, IBlock block, int position)
        {
            if (block.IsNull(position))
            {
                return NULL_HASH_CODE;
            }

            return type.Hash(block, position);
        }

        public static void CheckElementNotNull(bool isNull, string errorMsg)
        {
            if (isNull)
            {
                throw new PrestoException(errorMsg, new NotSupportedException());
            }
        }

        #endregion
    }
}
