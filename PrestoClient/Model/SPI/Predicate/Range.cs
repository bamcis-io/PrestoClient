using BAMCIS.PrestoClient.Model.SPI.Type;
using System;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.Range.java
    /// 
    /// A Range of values across the continuous space defined by the types of the Markers
    /// </summary>
    public sealed class Range
    {
        #region Public Properties

        public Marker Low { get; }

        public Marker High { get; }

        public IType Type
        {
            get
            {
                return this.Low.Type;
            }
        }

        #endregion

        #region Constructors

        public Range(Marker low, Marker high)
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

        public bool IsSingleValue()
        {
            return this.Low.Bound == Bound.EXACTLY && this.Low.Equals(this.High);
        }

        public object GetSingleValue()
        {
            if (!this.IsSingleValue())
            {
                throw new InvalidOperationException("Range does not have just a single value.");
            }

            return this.Low.GetValue();
        }

        public bool IsAll()
        {
            return this.Low.IsLowerUnbounded() && this.High.IsUpperUnbounded();
        }

        public bool Includes(Marker marker)
        {
            if (marker == null)
            {
                throw new ArgumentNullException("marker");
            }

            this.CheckTypeCompatibility(marker);

            return this.Low.CompareTo(marker) <= 0 && this.High.CompareTo(marker) >= 0;
        }

        public bool Contains(Range other)
        {
            this.CheckTypeCompatibility(other);

            return this.Low.CompareTo(other.Low) <= 0 &&
                   this.High.CompareTo(other.High) >= 0;
        }

        public Range Span(Range other)
        {
            this.CheckTypeCompatibility(other);

            Marker LowMarker = Marker.Min(this.Low, other.Low);
            Marker HighMarker = Marker.Max(this.High, other.High);
            return new Range(LowMarker, HighMarker);
        }

        public bool Overlaps(Range other)
        {
            this.CheckTypeCompatibility(other);

            return this.Low.CompareTo(other.High) <= 0 &&
                other.Low.CompareTo(this.High) <= 0;
        }

        public Range Intersect(Range other)
        {
            this.CheckTypeCompatibility(other);

            if (!this.Overlaps(other))
            {
                throw new ArgumentException("Cannot intersect non-overlapping ranges");
            }

            Marker LowMarker = Marker.Max(this.Low, other.Low);
            Marker HighMarker = Marker.Min(this.High, other.High);
            return new Range(LowMarker, HighMarker);
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

            Range Other = (Range)obj;

            return this.Low.Equals(Other.Low) &&
                    this.High.Equals(Other.High);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Low, this.High);
        }

        public string ToString(IConnectorSession session)
        {
            StringBuilder SB = new StringBuilder();

            if (this.IsSingleValue())
            {
                SB.Append("[").Append(this.Low.GetPrintableValue(session)).Append("]");
            }
            else
            {
                SB.Append(this.Low.Bound == Bound.EXACTLY ? "[" : "(");
                SB.Append(this.Low.IsLowerUnbounded() ? "<min>" : this.Low.GetPrintableValue(session));
                SB.Append(", ");
                SB.Append(this.High.IsUpperUnbounded() ? "<max>" : this.High.GetPrintableValue(session));
                SB.Append(this.High.Bound == Bound.EXACTLY ? "]" : ")");
            }

            return SB.ToString();
        }

        #endregion

        #region Public Static Methods

        public static Range All(IType type)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.UpperUnbounded(type));
        }

        public static Range GreaterThan(IType type, object low)
        {
            return new Range(Marker.Above(type, low), Marker.UpperUnbounded(type));
        }

        public static Range GreaterThanOrEqual(IType type, object low)
        {
            return new Range(Marker.Exactly(type, low), Marker.UpperUnbounded(type));
        }

        public static Range LessThan(IType type, object high)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.Below(type, high));
        }

        public static Range LessThanOrEqualTo(IType type, object high)
        {
            return new Range(Marker.LowerUnbounded(type), Marker.Exactly(type, high));
        }

        public static Range Equal(IType type, object value)
        {
            return new Range(Marker.Exactly(type, value), Marker.Exactly(type, value));
        }

        public static Range Create(IType type, object low, bool lowInclusive, object high, bool highInclusive)
        {
            Marker LowMarker = lowInclusive ? Marker.Exactly(type, low) : Marker.Above(type, low);
            Marker HighMarker = highInclusive ? Marker.Exactly(type, high) : Marker.Below(type, high);
            return new Range(LowMarker, HighMarker);
        }

        #endregion

        #region Private Methods

        private void CheckTypeCompatibility(Range range)
        {
            if (!this.Type.Equals(range.Type))
            {
                throw new ArgumentException($"Mismatched Range types: {this.Type} vs {range.Type}.");
            }
        }

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!this.Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Marker of {marker.Type} does not match Range of {this.Type}.");
            }
        }

        #endregion
    }
}
