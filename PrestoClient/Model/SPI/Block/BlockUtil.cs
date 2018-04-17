using System;

namespace BAMCIS.PrestoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.BlockUtil.java
    /// </summary>
    public sealed class BlockUtil
    {
        #region Public Fields

        public static readonly int MAX_ARRAY_SIZE = Int32.MaxValue - 8;

        #endregion

        #region Private Fields

        private static readonly double BLOCK_RESET_SKEW = 1.25;

        private static readonly int DEFAULT_CAPACITY = 64;

        #endregion

        #region Constructors

        private BlockUtil()
        { }

        #endregion

        #region Public Static Methods

        public static void CheckArrayRange(int[] array, int offset, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (offset < 0 || length < 0 || offset + length > array.Length)
            {
                throw new ArgumentOutOfRangeException($"Invalid offset {offset} and length {length} in array with {array.Length} elements.");
            }
        }

        public static void CheckValidRegion(int positionCount, int positionOffset, int length)
        {
            if (positionOffset < 0 || length < 0 || positionOffset + length > positionCount)
            {
                throw new ArgumentOutOfRangeException($"Invalid position {positionOffset} and length {length} in block with {positionCount} positions.");
            }
        }

        public static void CheckValidPosition(int position, int positionCount)
        {
            if (position < 0 || position >= positionCount)
            {
                throw new ArgumentException($"Invalid position {position} in block with {positionCount} positions.");
            }
        }

        public static int CalculateNewArraySize(int currentSize)
        {
            // grow array by 50%
            long NewSize = currentSize + (currentSize >> 1);

            // verify new size is within reasonable bounds
            if (NewSize < DEFAULT_CAPACITY)
            {
                NewSize = DEFAULT_CAPACITY;
            }
            else if (NewSize > MAX_ARRAY_SIZE)
            {
                NewSize = MAX_ARRAY_SIZE;

                if (NewSize == currentSize)
                {
                    throw new ArgumentException($"Can not grow array beyond '{MAX_ARRAY_SIZE}'.");
                }
            }

            return (int)NewSize;
        }

        public static int CalculateBlockResetSize(int currentSize)
        {
            long NewSize = (long)Math.Ceiling(currentSize * BLOCK_RESET_SKEW);

            // verify new size is within reasonable bounds
            if (NewSize < DEFAULT_CAPACITY)
            {
                NewSize = DEFAULT_CAPACITY;
            }
            else if (NewSize > MAX_ARRAY_SIZE)
            {
                NewSize = MAX_ARRAY_SIZE;
            }

            return (int)NewSize;
        }

        public static int CalculateBlockResetBytes(int currentBytes)
        {
            long NewBytes = (long)Math.Ceiling(currentBytes * BLOCK_RESET_SKEW);

            if (NewBytes > MAX_ARRAY_SIZE)
            {
                return MAX_ARRAY_SIZE;
            }

            return (int)NewBytes;
        }

        /// <summary>
        /// Recalculate the offsets array for the specified range.
        /// The returned offsets array contains length + 1 integers
        /// with the first value set to 0.
        /// If the range matches the entire offsets> array, the input array will be returned.
        /// </summary>
        /// <param name="offsets"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int[] CompactOffsets(int[] offsets, int index, int length)
        {
            if (index == 0 && offsets.Length == length + 1)
            {
                return offsets;
            }

            int[] NewOffsets = new int[length + 1];

            for (int i = 1; i <= length; i++)
            {
                NewOffsets[i] = offsets[index + i] - offsets[index];
            }

            return NewOffsets;
        }

        /// <summary>
        /// Returns a slice containing values in the specified range of the specified slice.
        /// If the range matches the entire slice, the input slice will be returned.
        /// Otherwise, a copy will be returned.
        /// </summary>
        /// <param name="slice"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static Slice CompactSlice(Slice slice, int index, int length)
        {
            if (slice.IsCompact() && index == 0 && length == slice.Size)
            {
                return slice;
            }

            return Slices.CopyOf(slice, index, length);
        }

        static bool[] CompactArray(bool[] array, int index, int length)
        {
            return CompactArray<bool>(array, index, length);
        }

        static byte[] CompactArray(byte[] array, int index, int length)
        {
            return CompactArray<byte>(array, index, length);
        }

        static short[] CompactArray(short[] array, int index, int length)
        {
            return CompactArray<short>(array, index, length);
        }

        static int[] CompactArray(int[] array, int index, int length)
        {
            return CompactArray<int>(array, index, length);
        }

        static long[] CompactArray(long[] array, int index, int length)
        {
            return CompactArray<long>(array, index, length);
        }

        static T[] CompactArray<T>(T[] array, int index, int length)
        {
            if (index == 0 && length == array.Length)
            {
                return array;
            }

            T[] NewArray = new T[length];

            Array.Copy(array, index, NewArray, 0, length);

            return NewArray;
        }

        public static bool ArraySame(object[] array1, object[] array2)
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
            {
                throw new ArgumentException("array1 and array2 cannot be null and should have same length");
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (!Object.Equals(array1[i], array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
