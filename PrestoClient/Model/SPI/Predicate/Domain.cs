using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.Domain.java
    /// </summary>
    public sealed class Domain
    {
        #region Public Properties

        public IValueSet Values { get; }

        public bool NullAllowed { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Domain(IValueSet values, bool nullAllowed)
        {
            this.Values = values ?? throw new ArgumentNullException("values");
            this.NullAllowed = nullAllowed;
        }

        #endregion

        #region Public Methods

        public IType GetPrestoType()
        {
            return this.Values.GetPrestoType();
        }

        public bool IsNone()
        {
            return this.Values.IsNone() && !this.NullAllowed;
        }

        public bool IsAll()
        {
            return this.Values.IsAll() && this.NullAllowed;
        }

        public bool IsSingleValue()
        {
            return !this.NullAllowed && Values.IsSingleValue();
        }

        public bool IsNullableSingleValue()
        {
            if (this.NullAllowed)
            {
                return this.Values.IsNone();
            }
            else
            {
                return this.Values.IsSingleValue();
            }
        }

        public bool IsOnlyNull()
        {
            return this.Values.IsNone() && this.NullAllowed;
        }

        public object GetSingleValue()
        {
            if (!this.IsSingleValue())
            {
                throw new InvalidOperationException("Domain is not a single value.");
            }

            return this.Values.GetSingleValue();
        }

        public object GetNullableSingleValue()
        {
            if (!this.IsNullableSingleValue())
            {
                throw new InvalidOperationException("Domain is not a nullable single value.");
            }

            if (this.NullAllowed)
            {
                return null;
            }
            else
            {
                return this.Values.GetSingleValue();
            }
        }

        public bool IncludesNullableValue(object value)
        {
            return (value == null ? this.NullAllowed : this.Values.ContainsValue(value));
        }

        public bool Overlaps(Domain other)
        {
            this.CheckCompatibility(other);
            return !this.Intersect(other).IsNone();
        }

        public bool Contains(Domain other)
        {
            this.CheckCompatibility(other);
            return this.Union(other).Equals(this);
        }

        public Domain Intersect(Domain other)
        {
            this.CheckCompatibility(other);
            return new Domain(this.Values.Intersect(other.Values), this.NullAllowed && other.NullAllowed);
        }

        public Domain Union(Domain other)
        {
            this.CheckCompatibility(other);
            return new Domain(this.Values.Union(other.Values), this.NullAllowed || other.NullAllowed);
        }

        public static Domain Union(IEnumerable<Domain> domains)
        {
            if (!domains.Any())
            {
                throw new ArgumentException("Domains cannot be empty for union.");
            }

            if (domains.Count() == 1)
            {
                return domains.ElementAt(0);
            }

            bool NullAllowed = domains.Any(x => x.NullAllowed);
            IEnumerable<IValueSet> ValueSets = domains.Select(x => x.Values);
            IValueSet UnionedValues = ValueSets.First().Union(ValueSets.Skip(1));

            return new Domain(UnionedValues, NullAllowed);
        }

        public Domain Complement()
        {
            return new Domain(this.Values.Complement(), !this.NullAllowed);
        }

        public Domain Substract(Domain other)
        {
            this.CheckCompatibility(other);
            return new Domain(this.Values.Subtract(other.Values), this.NullAllowed && !other.NullAllowed);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Values, this.NullAllowed);
        }

        public string ToString(IConnectorSession session)
        {
            return $"[ {(this.NullAllowed ? "NULL, " : "")}{this.Values.ToString(session)} ]";
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

            Domain Other = (Domain)obj;

            return Object.Equals(this.Values, Other.Values) &&
                    this.NullAllowed == Other.NullAllowed;
        }

        /// <summary>
        ///  Reduces the number of discrete components in the Domain if there are too many.
        /// </summary>
        /// <returns></returns>
        public Domain Simplify()
        {
            IValueSet SimplifiedValueSet = this.Values.GetValuesProcessor().Transform<IValueSet>(ranges =>
            {
                if (ranges.GetOrderedRanges().Count() <= 32)
                {
                    return null;
                }

                return ValueSet.OfRanges(ranges.GetSpan());
            },
            discreteValues =>
            {
                if (discreteValues.GetValues().Count() <= 32)
                {
                    return null;
                }

                return ValueSet.All(this.Values.GetPrestoType());
            },
            allOrNone =>
            {
                return null;
            });

            if (SimplifiedValueSet == null)
            {
                SimplifiedValueSet = this.Values;
            }

            return Domain.Create(SimplifiedValueSet, this.NullAllowed);
        }

        #endregion

        #region Public Static Methods

        public static Domain Create(IValueSet values, bool nullAllowed)
        {
            return new Domain(values, nullAllowed);
        }

        public static Domain None(IType type)
        {
            return new Domain(ValueSet.None(type), false);
        }

        public static Domain All(IType type)
        {
            return new Domain(ValueSet.All(type), true);
        }

        public static Domain OnlyNull(IType type)
        {
            return new Domain(ValueSet.None(type), true);
        }

        public static Domain NotNull(IType type)
        {
            return new Domain(ValueSet.All(type), false);
        }

        public static Domain SingleValue(IType type, object value)
        {
            return new Domain(ValueSet.Of(type, value), false);
        }

        public static Domain MultipleValues(IType type, IEnumerable<object> values)
        {
            if (values == null || !values.Any())
            {
                throw new ArgumentException("values", "Values cannot be null or empty.");
            }

            if (values.Count() == 1)
            {
                return SingleValue(type, values.ElementAt(0));
            }

            return new Domain(ValueSet.Of(type, values.ElementAt(0), values.Skip(1)), false);
        }

        #endregion

        #region Private Methods

        public void CheckCompatibility(Domain domain)
        {
            if (!this.GetPrestoType().Equals(domain.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched domain types: {this.GetPrestoType()} vs {domain.GetPrestoType()}.");
            }

            if (this.Values.GetType() != domain.Values.GetType())
            {
                throw new ArgumentException($"Mismatched domain value set types: {this.Values.GetType().Name} vs {domain.Values.GetType().Name}.");
            }
        }

        #endregion
    }
}
