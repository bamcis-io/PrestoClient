using BAMCIS.PrestoClient.Model.SPI.Type;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.SortedRangeSet.java
    /// </summary>
    public sealed class SortedRangeSet : IValueSet, IRanges, IValuesProcessor
    {
        #region Public Properties

        public IType Type { get; }

        public SortedDictionary<Marker, Range> LowIndexedRanges { get; }

        public IEnumerable<Range> Ranges
        {
            get
            {
                return this.LowIndexedRanges.Values;
            }
        }

        #endregion

        #region Constructors

        public SortedRangeSet(IType type, SortedDictionary<Marker, Range> lowIndexedRanges)
        {
            this.Type = type ?? throw new ArgumentNullException("type");
            this.LowIndexedRanges = lowIndexedRanges ?? throw new ArgumentNullException("lowIndexedRanges");

            if (!this.Type.IsOrderable())
            {
                throw new ArgumentException($"Type is not orderable: {this.Type.ToString()}.", "type");
            }

        }

        #endregion

        #region Public Static Methods

        public static SortedRangeSet None(IType type)
        {
            return CopyOf(type, new Range[0]);
        }

        public static SortedRangeSet All(IType type)
        {
            return CopyOf(type, new Range[] { Range.All(type) });
        }

        /// <summary>
        /// Provided Ranges are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static SortedRangeSet CopyOf(IType type, IEnumerable<Range> ranges)
        {
            ranges = ranges.OrderBy(x => x.Low);

            Range Current = null;

            SortedDictionary<Marker, Range> Result = new SortedDictionary<Marker, Range>();

            foreach (Range Next in ranges)
            {
                if (Current == null)
                {
                    Current = Next;
                    continue;
                }

                if (Current.Overlaps(Next) || Current.High.IsAdjacent(Next.Low))
                {
                    Current = Current.Span(Next);
                }
                else
                {
                    Result.Add(Current.Low, Current);
                }
            }

            if (Current != null)
            {
                Result.Add(Current.Low, Current);
            }

            return new SortedRangeSet(type, Result);
        }

        /// <summary>
        /// Provided discrete values that are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="type"></param>
        /// <param name="first"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public static SortedRangeSet Of(IType type, object first, params object[] rest)
        {
            return CopyOf(type, rest.Concat(new object[] { first }).Select(x => Range.Equal(type, x)));
        }

        /// <summary>
        /// Provided Ranges are unioned together to form the SortedRangeSet
        /// </summary>
        /// <param name="first"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public static SortedRangeSet Of(Range first, params Range[] rest)
        {
            return CopyOf(first.Type, rest.Concat(new Range[] { first }));
        }

        #endregion

        #region Public Methods

        public bool IsNone()
        {
            return !this.LowIndexedRanges.Any();
        }

        public bool IsAll()
        {
            return this.LowIndexedRanges.Count == 1 && this.LowIndexedRanges.Values.First().IsAll();
        }

        public bool IsSingleValue()
        {
            return this.LowIndexedRanges.Count == 1 && this.LowIndexedRanges.Values.First().IsSingleValue();
        }

        public object GetSingleValue()
        {
            if (!this.IsSingleValue())
            {
                throw new IndexOutOfRangeException("SortedRAngeSet does not have just a single value.");
            }

            return this.LowIndexedRanges.Values.First().GetSingleValue();
        }

        public bool ContainsValue(object value)
        {
            return this.IncludesMarker(Marker.Exactly(this.Type, value));
        }

        public IRanges GetRanges()
        {
            return (IRanges)this;
        }

        public IValuesProcessor GetValuesProcessor()
        {
            return this;
        }

        public T Transform<T>(Func<IRanges, T> rangesFunction, Func<IDiscreteValues, T> valuesFunction, Func<IAllOrNone, T> allOrNoneFunction)
        {
            return rangesFunction.Invoke(this.GetRanges());
        }

        public void Consume(Consumer<IRanges> rangesConsumer, Consumer<IDiscreteValues> valuesConsumer, Consumer<IAllOrNone> allOrNoneConsumer)
        {
            rangesConsumer.Accept(this.GetRanges());
        }

        public int GetRangeCount()
        {
            return this.LowIndexedRanges.Count;
        }

        public IEnumerable<Range> GetOrderedRanges()
        {
            return this.Ranges;
        }

        public Range GetSpan()
        {
            if (!this.LowIndexedRanges.Any())
            {
                throw new InvalidOperationException("Cannot get span if no ranges exists");
            }

            return this.LowIndexedRanges.First().Value.Span(this.LowIndexedRanges.Last().Value);
        }

        public IType GetPrestoType()
        {
            return this.Type;
        }

        public IValueSet Intersect(IValueSet other)
        {
            SortedRangeSet OtherRangeSet = this.CheckCompatibility(other);

            Builder Builder = new Builder(this.Type);

            IEnumerator<Range> Iterator1 = this.GetOrderedRanges().GetEnumerator();
            IEnumerator<Range> Iterator2 = OtherRangeSet.GetOrderedRanges().GetEnumerator();

            if (Iterator1.MoveNext() && Iterator2.MoveNext())
            {
                Range Range1 = Iterator1.Current;
                Range Range2 = Iterator2.Current;

                while (true)
                {
                    if (Range1.Overlaps(Range2))
                    {
                        Builder.Add(Range1.Intersect(Range2));
                    }

                    if (Range1.High.CompareTo(Range2.High) <= 0)
                    {
                        if (!Iterator1.MoveNext())
                        {
                            break;
                        }

                        Range1 = Iterator1.Current;
                    }
                    else
                    {
                        if (!Iterator2.MoveNext())
                        {
                            break;
                        }

                        Range2 = Iterator2.Current;
                    }
                }
            }

            return Builder.Build();
        }

        public IValueSet Union(IValueSet other)
        {
            SortedRangeSet OtherRangeSet = this.CheckCompatibility(other);

            return new Builder(this.Type)
                .AddAll(this.GetOrderedRanges())
                .AddAll(OtherRangeSet.GetOrderedRanges())
                .Build();
        }

        public IValueSet Union(IEnumerable<IValueSet> valueSets)
        {
            Builder Builder = new Builder(this.Type);
            Builder.AddAll(this.GetOrderedRanges());

            foreach (IValueSet Set in valueSets)
            {
                Builder.AddAll(this.CheckCompatibility(Set).GetOrderedRanges());
            }

            return Builder.Build();
        }

        public IValueSet Complement()
        {
            Builder Builder = new Builder(this.Type);

            if (!this.LowIndexedRanges.Any())
            {
                return Builder.Add(Range.All(this.Type)).Build();
            }

            IEnumerable<Range> RangeIterator = this.LowIndexedRanges.Values;

            Range FirstRange = RangeIterator.First();

            if (!FirstRange.Low.IsLowerUnbounded())
            {
                Builder.Add(new Range(Marker.LowerUnbounded(this.Type), FirstRange.Low.LesserAdjacent()));
            }

            Range PreviousRange = FirstRange;

            foreach (Range Next in RangeIterator.Skip(1))
            {
                Marker LowMarker = PreviousRange.High.GreaterAdjacent();
                Marker HighMarker = Next.Low.LesserAdjacent();

                Builder.Add(new Range(LowMarker, HighMarker));

                PreviousRange = Next;
            }

            Range LastRange = PreviousRange;

            if (!LastRange.High.IsUpperUnbounded())
            {
                Builder.Add(new Range(LastRange.High.GreaterAdjacent(), Marker.UpperUnbounded(this.Type)));
            }

            return Builder.Build();
        }

        public bool Overlaps(IValueSet other)
        {
            return !this.Intersect(other).IsNone();
        }

        public bool Contains(IValueSet other)
        {
            return this.Union(other).Equals(this);
        }

        public IValueSet Subtract(IValueSet other)
        {
            return this.Intersect(other.Complement());
        }

        public string ToString(IConnectorSession session)
        {
            return $"[{String.Join(", ", this.LowIndexedRanges.Values.Select(x => x.ToString(session)))}]";
        }

        public bool IncludesMarker(Marker marker)
        {
            KeyValuePair<Marker, Range> FloorEntry = LowIndexedRanges.FloorEntry(marker);
            return !FloorEntry.Equals(default(KeyValuePair<Marker, Range>)) && FloorEntry.Value.Includes(marker);
        }

        #endregion

        #region Private Methods

        private SortedRangeSet CheckCompatibility(IValueSet other)
        {
            if (!this.Type.Equals(other.GetPrestoType()))
            {
                throw new ArgumentException($"Mismatched types: {this.Type} vs {other.GetPrestoType()}.");
            }

            if (!(other is SortedRangeSet)) {
                throw new ArgumentException($"ValueSet is not a SortedRangeSet: {other.GetType()}.");
            }

            return (SortedRangeSet)other;
        }

        private void CheckTypeCompatibility(Marker marker)
        {
            if (!this.Type.Equals(marker.Type))
            {
                throw new ArgumentException($"Marker of {marker.Type} does not match SortedRangeSet of {this.Type}.");
            }
        }


        #endregion

        #region Internal Classes

        internal class Builder
        {
            private IType Type { get; }

            private List<Range> Ranges { get; }

            internal Builder(IType type)
            {
                this.Type = type ?? throw new ArgumentNullException("type");

                if (!this.Type.IsOrderable())
                {
                    throw new ArgumentException($"Type is not orderable: {this.Type.ToString()}.");
                }

                this.Ranges = new List<Range>();
            }

            internal Builder Add(Range range)
            {
                if (!this.Type.Equals(range.Type))
                {
                    throw new ArgumentException($"Range type {range.Type.ToString()} does not match builder type {this.Type.ToString()}.");
                }

                Ranges.Add(range);
                return this;
            }

            internal Builder AddAll(IEnumerable<Range> ranges)
            {
                this.Ranges.AddRange(ranges);

                return this;
            }

            internal SortedRangeSet Build()
            {
                this.Ranges.Sort(new Comparison<Range>(delegate(Range x, Range y)
                {
                    return x.Low.CompareTo(y.Low);
                }));

                SortedDictionary<Marker, Range> Result = new SortedDictionary<Marker, Range>();

                Range Current = null;

                foreach (Range Next in this.Ranges)
                {
                    if (Current == null)
                    {
                        Current = Next;
                        continue;
                    }

                    if (Current.Overlaps(Next) || Current.High.IsAdjacent(Next.Low))
                    {
                        Current = Current.Span(Next);
                    }
                    else
                    {
                        Result.Add(Current.Low, Current);
                    }
                }

                if (Current != null)
                {
                    Result.Add(Current.Low, Current);
                }

                return new SortedRangeSet(this.Type, Result);
            }
        }

        #endregion
    }
}
