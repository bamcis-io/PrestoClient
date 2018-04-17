using System;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From io.airlift.slice.Slices.java
    /// </summary>
    public class Slices
    {
        #region Public Fields

        public static readonly Slice EMPTY_SLICE = new Slice();

        #endregion

        #region Private Fields

        private static readonly int MAX_ARRAY_SIZE = Int32.MaxValue - 8;

        private static readonly int SLICE_ALLOC_THRESHOLD = 524288; // 2^19

        private static readonly double SLICE_ALLOW_SKEW = 1.25; // Must be > 1

        #endregion

        #region Constructors

        private Slices()
        { }

        #endregion

        #region Public Static Methods

        public static Slice Allocate(int capacity)
        {
            if (capacity == 0)
            {
                return EMPTY_SLICE;
            }

            ParameterCheck.Check(capacity <= MAX_ARRAY_SIZE, $"Cannot allocate slice largert than {MAX_ARRAY_SIZE} bytes.");

            return new Slice(new byte[capacity]);
        }

        public static Slice CopyOf(Slice slice, int offset, int length)
        {
            Preconditions.CheckPositionIndexes(offset, offset + length, slice.Length);

            Slice Copy = Slices.Allocate(length);
            Copy.SetBytes(0, slice, offset, length);

            return Copy;
        }

        #endregion
    }
}