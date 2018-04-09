using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.Marker.java
    /// </summary>
    public class Marker<T> : IComparable<Marker<T>>
    {
        #region Public Properties

        public System.Type Type { get; }

        public IComparable<T> Value { get; }

        public Bound Bound { get; }

        #endregion

        #region Constructors

        private Marker(System.Type type, IComparable<T> value, Bound bound)
        {
            this.Value = value;
            this.Bound = bound;
            this.Type = type;
        }

        [JsonConstructor]
        public Marker(SerializableNativeValue<T> value, Bound bound) : this(typeof(T), value.Value, bound)
        {
        }

        #endregion

        #region Public Static Methods

        public static Marker<T> UpperUnbounded()
        {
            return new Marker<T>(typeof(T), null, Bound.BELOW);
        }

        public static Marker<T> LowerUnbounded()
        {
            return new Marker<T>(typeof(T), null, Bound.ABOVE);
        }

        public static Marker<T> Above(IComparable<T> value)
        {
            return new Marker<T>(typeof(T), value, Bound.ABOVE);
        }

        public static Marker<T> Exactly(IComparable<T> value)
        {
            return new Marker<T>(typeof(T), value, Bound.EXACTLY);
        }

        public static Marker<T> Below(IComparable<T> value)
        {
            return new Marker<T>(typeof(T), value, Bound.BELOW);
        }

        public static Marker<T> Min(Marker<T> marker1, Marker<T> marker2)
        {
            return marker1.CompareTo(marker2) <= 0 ? marker1 : marker2;
        }

        public static Marker<T> Max(Marker<T> marker1, Marker<T> marker2)
        {
            return marker1.CompareTo(marker2) >= 0 ? marker1 : marker2;
        }

        #endregion

        #region Public Methods

        public int CompareTo(Marker<T> other)
        {
            if (this.IsUpperUnbounded())
            {
                return other.IsUpperUnbounded() ? 0 : 1;
            }

            if (this.IsLowerUnbounded())
            {
                return other.IsLowerUnbounded() ? 0 : -1;
            }

            if (other.IsUpperUnbounded())
            {
                return -1;
            }

            if (other.IsLowerUnbounded())
            {
                return 1;
            }
            // INVARIANT: value and o.value not null

            int Compare = this.Value.CompareTo((T)other.Value);

            if (Compare == 0)
            {
                if (this.Bound == other.Bound)
                {
                    return 0;
                }
                if (this.Bound == Bound.BELOW)
                {
                    return -1;
                }
                if (this.Bound == Bound.ABOVE)
                {
                    return 1;
                }

                // INVARIANT: bound == EXACTLY
                return (other.Bound == Bound.BELOW) ? 1 : -1;
            }

            return Compare;
        }

        public bool IsUpperUnbounded()
        {
            return this.Value == null && this.Bound == Bound.BELOW;
        }

        public bool IsLowerUnbounded()
        {
            return this.Value == null && this.Bound == Bound.ABOVE;
        }

        /// <summary>
        /// Adjacency is defined by two Markers being infinitesimally close to each other.
        /// This means they must share the same value and have adjacent Bounds.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAdjacent(Marker<T> other)
        {
            if (this.IsUpperUnbounded() || this.IsLowerUnbounded() || other.IsUpperUnbounded() || other.IsLowerUnbounded())
            {
                return false;
            }

            if (this.Value.CompareTo((T)other.Value) != 0)
            {
                return false;
            }

            return (this.Bound == Bound.EXACTLY && other.Bound != Bound.EXACTLY) ||
                   (this.Bound != Bound.EXACTLY && other.Bound == Bound.EXACTLY);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
