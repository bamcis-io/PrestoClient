using BAMCIS.PrestoClient.Model.SPI.Block;
using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Marker.java
    /// </summary>
    public class Marker : IComparable<Marker>
    {
        #region Public Properties

        public IType Type { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Optional]
        public IBlock ValueBlock { get; }

        public Bound Bound { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// LOWER UNBOUNDED is specified with an empty value and a ABOVE bound
        /// UPPER UNBOUNDED is specified with an empty value and a BELOW bound
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueBlock"></param>
        /// <param name="bound"></param>
        [JsonConstructor]
        public Marker(IType type, IBlock valueBlock, Bound bound)
        {
            this.ValueBlock = valueBlock ?? throw new ArgumentNullException("valueBlock");
            this.Bound = bound;
            this.Type = type;

            if (!this.Type.IsOrderable())
            {
                throw new ArgumentException("Type must be orderable");
            }

            if (this.ValueBlock == null && this.Bound == Bound.EXACTLY)
            {
                throw new ArgumentException("Cannot be equal to unbounded.");
            }

            if (this.ValueBlock != null && this.ValueBlock.GetPositionCount() != 1)
            {
                throw new ArgumentException("Value block should only have one position.");
            }
        }

        #endregion

        #region Public Static Methods

        public static Marker UpperUnbounded(IType type)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, null, Bound.BELOW);
        }

        public static Marker LowerUnbounded(IType type)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, null, Bound.ABOVE);
        }

        public static Marker Above(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.ABOVE);
        }

        public static Marker Exactly(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.EXACTLY);
        }

        public static Marker Below(IType type, object value)
        {
            ParameterCheck.NonNull<IType>(type, "type");
            return Marker.Create(type, value, Bound.BELOW);
        }

        public static Marker Min(Marker marker1, Marker marker2)
        {
            return marker1.CompareTo(marker2) <= 0 ? marker1 : marker2;
        }

        public static Marker Max(Marker marker1, Marker marker2)
        {
            return marker1.CompareTo(marker2) >= 0 ? marker1 : marker2;
        }

        #endregion

        #region Public Methods

        public object GetValue()
        {
            if (this.ValueBlock == null)
            {
                throw new InvalidOperationException("No value to get.");
            }

            return Utils.BlockToNativeValue(this.Type, this.ValueBlock);
        }

        public object GetPrintableValue(IConnectorSession session)
        {
            if (this.ValueBlock == null)
            {
                throw new InvalidOperationException("No value to get.");
            }

            return this.Type.GetObjectValue(session, this.ValueBlock, 0);
        }

        public int CompareTo(Marker other)
        {
            this.CheckTypeCompatibility(other);

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

            int Compare = this.Type.CompareTo(this.ValueBlock, 0, other.ValueBlock, 0);

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
            return this.ValueBlock == null && this.Bound == Bound.BELOW;
        }

        public bool IsLowerUnbounded()
        {
            return this.ValueBlock == null && this.Bound == Bound.ABOVE;
        }

        /// <summary>
        /// Adjacency is defined by two Markers being infinitesimally close to each other.
        /// This means they must share the same value and have adjacent Bounds.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAdjacent(Marker other)
        {
            if (this.IsUpperUnbounded() || this.IsLowerUnbounded() || other.IsUpperUnbounded() || other.IsLowerUnbounded())
            {
                return false;
            }

            if (this.Type.CompareTo(this.ValueBlock, 0, other.ValueBlock, 0) != 0)
            {
                return false;
            }

            return (this.Bound == Bound.EXACTLY && other.Bound != Bound.EXACTLY) ||
                   (this.Bound != Bound.EXACTLY && other.Bound == Bound.EXACTLY);
        }

        public Marker GreaterAdjacent()
        {
            if (this.ValueBlock == null)
            {
                throw new InvalidOperationException("No marker adjacent to unbounded.");
            }

            switch (this.Bound)
            {
                case Bound.BELOW:
                    {
                        return new Marker(this.Type, this.ValueBlock, Bound.EXACTLY);
                    }
                case Bound.EXACTLY:
                    {
                        return new Marker(this.Type, this.ValueBlock, Bound.ABOVE);
                    }
                case Bound.ABOVE:
                    {
                        throw new InvalidOperationException("No greater marker adjacent to an ABOVE bound.");
                    }
                default:
                    {
                        throw new InvalidOperationException($"Unsupported type: {this.Bound.ToString()}");
                    }
            }
        }

        public Marker LesserAdjacent()
        {
            if (this.ValueBlock == null)
            {
                throw new InvalidOperationException("No marker adjacent to unbounded.");
            }

            switch (this.Bound)
            {
                case Bound.BELOW:
                    {
                        throw new InvalidOperationException("No lesser marker adjacent to a BELOW bound.");
                        
                    }
                case Bound.EXACTLY:
                    {
                        return new Marker(this.Type, this.ValueBlock, Bound.BELOW);
                    }
                case Bound.ABOVE:
                    {
                        return new Marker(this.Type, this.ValueBlock, Bound.EXACTLY);
                    }
                default:
                    {
                        throw new InvalidOperationException($"Unsupported type: {this.Bound.ToString()}");
                    }
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int Hash = Hashing.Hash(this.Type, this.Bound);

                if (this.ValueBlock != null)
                {

                    Hash = Hash * 31 + (int)this.Type.Hash(this.ValueBlock, 0);
                }

                return Hash;
            }
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
            Marker Other = (Marker)obj;

            return Object.Equals(this.Type, Other.Type)
                    && Object.Equals(this.Bound, Other.Bound)
                    && ((this.ValueBlock != null) == (Other.ValueBlock != null))
                    && (this.ValueBlock == null || this.Type.EqualTo(this.ValueBlock, 0, Other.ValueBlock, 0));
        }

        public string ToString(IConnectorSession session)
        {
            StringBuilder SB = new StringBuilder("{");

            SB.Append("type=").Append(this.Type.ToString());
            SB.Append(", value=");

            if (this.IsLowerUnbounded())
            {
                SB.Append("<min>");
            }
            else if (this.IsUpperUnbounded())
            {
                SB.Append("<max>");
            }
            else
            {
                SB.Append(this.GetPrintableValue(session));
            }

            SB.Append(", bound=").Append(this.Bound.ToString());

            SB.Append("}");

            return SB.ToString();
        }

        #endregion

        #region Private Methods

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!this.Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Mismatched Marker types: {this.Type.ToString()} vs {marker.Type.ToString()}.");
            }
        }

        private static Marker Create(IType type, object value, Bound bound)
        {
            return new Marker(type, Utils.NativeValueToBlock(type, value), bound);
        }

        #endregion
    }
}
