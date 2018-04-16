using BAMCIS.PrestoClient.Model.SPI.Type;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.AllOrNoneValueSet.java
    /// </summary>
    public class AllOrNoneValueSet : IValueSet, IAllOrNone, IValuesProcessor
    {
        #region Private Fields

        private bool _All;

        #endregion

        #region Public Methods

        public IType Type { get; }

        #endregion

        #region Constructors

        public AllOrNoneValueSet(IType type, bool all)
        {
            this.Type = type ?? throw new ArgumentNullException("type");
            this._All = all;
        }

        #endregion

        #region Public Methods

        public bool IsAll()
        {
            return this._All;
        }

        public bool IsNone()
        {
            return !this._All;
        }

        public bool IsSingleValue()
        {
            return false;
        }

        public object GetSingleValue()
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(object value)
        {
            if (!value.GetType().Equals(this.Type.GetJavaType()))
            {
                throw new ArgumentException($"Value class {value.GetType().Name} does not match required type, {this.Type}.");
            }

            return this._All;
        }

        public IAllOrNone GetAllOrNone()
        {
            return this;
        }

        public IValuesProcessor GetValuesProcessor()
        {
            return this;
        }

        public T Transform<T>(Func<IRanges, T> rangesFunction, Func<IDiscreteValues, T> valuesFunction, Func<IAllOrNone, T> allOrNoneFunction)
        {
            return allOrNoneFunction.Invoke(this.GetAllOrNone());
        }

        public void Consume(Consumer<IRanges> rangesConsumer, Consumer<IDiscreteValues> valuesConsumer, Consumer<IAllOrNone> allOrNoneConsumer)
        {
            allOrNoneConsumer.Accept(this.GetAllOrNone());
        }

        public IValueSet CopyOf(IType type, IEnumerable<Range> values)
        {
            return new AllOrNoneValueSet(type, true);
        }

        public IValueSet Of(IType type, object first, params object[] rest)
        {
            return SortedRangeSet.Of(type, first, rest);
        }

        public override string ToString()
        {
            return $"[{ (_All ? "ALL" : "NONE")}]";
        }

        public IType GetPrestoType()
        {
            return this.Type;
        }

        public IValueSet Intersect(IValueSet other)
        {
            AllOrNoneValueSet OtherValueSet = this.CheckCompatibility(other);
            return new AllOrNoneValueSet(this.Type, this._All && OtherValueSet._All);
        }

        public IValueSet Union(IValueSet other)
        {
            AllOrNoneValueSet OtherValueSet = this.CheckCompatibility(other);
            return new AllOrNoneValueSet(this.Type, this._All || OtherValueSet._All);
        }

        public IValueSet Union(IEnumerable<IValueSet> valueSets)
        {
            IValueSet Current = this;

            foreach (IValueSet Set in valueSets)
            {
                Current = Current.Union(Set);
            }

            return Current;
        }

        public bool Overlaps(IValueSet other)
        {
            return !this.Intersect(other).IsNone();
        }

        public IValueSet Subtract(IValueSet other)
        {
            return this.Intersect(other.Complement());
        }

        public bool Contains(IValueSet other)
        {
            return this.Union(other).Equals(this);
        }

        public IValueSet Complement()
        {
            return new AllOrNoneValueSet(this.Type, !this._All);
        }

        public string ToString(IConnectorSession session)
        {
            return $"[{(this._All ? "ALL" : "NONE")}]";
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

            AllOrNoneValueSet Other = (AllOrNoneValueSet)obj;
            return Object.Equals(this.Type, Other.Type)
                    && this._All == Other._All;
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Type, this._All);
        }

        #endregion

        #region Public Static Methods

        public static AllOrNoneValueSet All(IType type)
        {
            return new AllOrNoneValueSet(type, true);
        }

        public static AllOrNoneValueSet None(IType type)
        {
            return new AllOrNoneValueSet(type, false);
        }

        #endregion

        #region Private Methods

        private AllOrNoneValueSet CheckCompatibility(IValueSet other)
        {
            if (this.Type.Equals(other.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched types: {this.Type} vs {other.GetPrestoType()}.");
            }

            if (!(other is AllOrNoneValueSet)) {
                throw new ArgumentException($"ValueSet is not a AllOrNoneValueSet: {other.GetType().Name}.");
            }

            return (AllOrNoneValueSet)other;
        }

        #endregion
    }
}
