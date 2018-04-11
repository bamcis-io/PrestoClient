using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.Range.java
    /// 
    /// TODO: In progress for TupleDomain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Range<T>
    {
        #region Public Properties

        public Marker<T> Low { get; }

        public Marker<T> High { get; }

        #endregion

        #region Constructors

        public Range(Marker<T> low, Marker<T> high)
        {
            if (low == null)
            {
                throw new ArgumentNullException("low");
            }

            if (high == null)
            {
                throw new ArgumentNullException("high");
            }

            if (low.IsUpperUnbounded())
            {
                throw new ArgumentException("Low cannot be upper unbounded.");
            }

            if (high.IsLowerUnbounded())
            {
                throw new ArgumentException("High cannot be lower unbounded.");
            }

            if (low.CompareTo(high) > 0)
            {
                throw new ArgumentOutOfRangeException("Low must be less than or equal to high.");
            }

            this.Low = low;
            this.High = high;
        }

        #endregion

        #region Public Methods

        public bool IsAll()
        {
            return this.Low.IsLowerUnbounded() && this.High.IsUpperUnbounded();
        }

        public bool Includes(Marker<T> marker)
        {
            return this.Low.CompareTo(marker) <= 0 && this.High.CompareTo(marker) >= 0;
        }

        public bool Contains(Range<T> other)
        {
            return this.Low.CompareTo(other.Low) <= 0 &&
                   this.High.CompareTo(other.High) >= 0;
        }

        public Range<T> Span(Range<T> other)
        {
            Marker<T> LowMarker = Marker<T>.Min(this.Low, other.Low);
            Marker<T> HighMarker = Marker<T>.Max(this.High, other.High);
            return new Range<T>(LowMarker, HighMarker);
        }

        public bool Overlaps(Range<T> other)
        {
            return this.Low.CompareTo(other.High) <= 0 &&
                other.Low.CompareTo(this.High) <= 0;
        }

        public Range<T> Intersect(Range<T> other)
        {
            if (!this.Overlaps(other))
            {
                throw new ArgumentException("Cannot intersect non-overlapping ranges");
            }

            Marker<T> LowMarker = Marker<T>.Max(this.Low, other.Low);
            Marker<T> HighMarker = Marker<T>.Min(this.High, other.High);
            return new Range<T>(LowMarker, HighMarker);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(T) != obj.GetType())
            {
                return false;
            }

            Range<T> Other = (Range<T>)obj;

            return this.Low.Equals(Other.Low) &&
                    this.High.Equals(Other.High);
        }

        #endregion

        #region Public Static Methods

        public static Range<T> All()
        {
            return new Range<T>(Marker<T>.LowerUnbounded(), Marker<T>.UpperUnbounded());
        }

        public static Range<T> GreaterThan(IComparable<T> low)
        {
            return new Range<T>(Marker<T>.Above(low), Marker<T>.UpperUnbounded());
        }

        public static Range<T> GreaterThanOrEqual(IComparable<T> low)
        {
            return new Range<T>(Marker<T>.Exactly(low), Marker<T>.UpperUnbounded());
        }

        public static Range<T> LessThan(IComparable<T> high)
        {
            return new Range<T>(Marker<T>.LowerUnbounded(), Marker<T>.Below(high));
        }

        public static Range<T> LessThanOrEqualTo(IComparable<T> high)
        {
            return new Range<T>(Marker<T>.LowerUnbounded(), Marker<T>.Exactly(high));
        }

        public static Range<T> Equal(IComparable<T> value)
        {
            return new Range<T>(Marker<T>.Exactly(value), Marker<T>.Exactly(value));
        }

        public static Range<T> Create(IComparable<T> low, bool lowInclusive, IComparable<T> high, bool highInclusive)
        {
            Marker<T> LowMarker = lowInclusive ? Marker<T>.Exactly(low) : Marker<T>.Above(low);
            Marker<T> HighMarker = highInclusive ? Marker<T>.Exactly(high) : Marker<T>.Below(high);
            return new Range<T>(LowMarker, HighMarker);
        }

        #endregion
    }
}
