using System;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From io.airlift.slice.Preconditions.java
    /// </summary>
    public sealed class Preconditions
    {
        #region Constructors

        private Preconditions()
        { }

        #endregion

        #region Public Static Methods

        public static int CheckPositionIndex(int index, int size)
        {
            return CheckPositionIndex(index, size, "index");
        }

        public static int CheckPositionIndex(int index, int size, string desc)
        {
            // Carefully optimized for execution by hotspot (explanatory comment above)
            if (index < 0 || index > size)
            {
                throw new ArgumentOutOfRangeException(BadPositionIndex(index, size, desc));
            }

            return index;
        }

        public static long CheckPositionIndex(long index, long size)
        {
            return CheckPositionIndex(index, size, "index");
        }

        public static long CheckPositionIndex(long index, long size, string desc)
        {
            // Carefully optimized for execution by hotspot (explanatory comment above)
            if (index < 0 || index > size)
            {
                throw new ArgumentOutOfRangeException(BadPositionIndex(index, size, desc));
            }
            return index;
        }

        public static void CheckPositionIndexes(int start, int end, int size)
        {
            // Carefully optimized for execution by hotspot (explanatory comment above)
            if (start < 0 || end < start || end > size)
            {
                throw new ArgumentOutOfRangeException(BadPositionIndexes(start, end, size));
            }
        }

        public static void Verify(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException();
            }
        }

        #endregion

        #region Private Methods

        private static string BadPositionIndex(long index, long size, string desc)
        {
            if (index < 0)
            {
                return $"{desc} ({index}) must not be negative.";
            }
            else if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", $"Negative size: {size}.");
            }
            else
            { 
                // index > size
                return $"{desc} ({index}) must not be greater than size ({size}).";
            }
        }

        private static string BadPositionIndex(int index, int size, string desc)
        {
            if (index < 0)
            {
                return $"{desc} ({index}) must not be negative.";
            }
            else if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", $"Negative size: {size}.");
            }
            else
            {
                // index > size
                return $"{desc} ({index}) must not be greater than size ({size}).";
            }
        }

        private static string BadPositionIndexes(int start, int end, int size)
        {
            if (start < 0 || start > size)
            {
                return BadPositionIndex(start, size, "start index");
            }

            if (end < 0 || end > size)
            {
                return BadPositionIndex(end, size, "end index");
            }

            // end < start
            return $"End index ({end}) must not be less than start index ({start}).";
        }


        #endregion
    }
}
