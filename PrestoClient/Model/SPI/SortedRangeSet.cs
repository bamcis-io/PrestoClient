using System;
using System.Collections;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.SortedRangeSet.java
    /// 
    /// TODO: In progress for TupleDomain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SortedRangeSet<T> : IEnumerable<Range<T>>
    {
        #region Public Properties

        public SortedDictionary<Marker<T>, Range<T>> LowIndexedRanges { get; }

        #endregion

        #region Constructors

        public SortedRangeSet(Range<T> first, params Range<T>[] args)
        {
            this.LowIndexedRanges = new SortedDictionary<Marker<T>, Range<T>>();

            if (first != null)
            {
                List<Range<T>> RangeList = new List<Range<T>>() { first };

                if (args != null)
                {
                    RangeList.AddRange(args);
                }

                foreach (Range<T> Item in RangeList)
                {
                    Range<T> Temp = Item;

                    KeyValuePair<Marker<T>, Range<T>> LowFloorEntry = this.LowIndexedRanges.FloorEntry(Temp.Low);

                    if (!LowFloorEntry.Equals(default(KeyValuePair<Marker<T>, Range<T>>)) && LowFloorEntry.Value.Overlaps(Temp))
                    {
                        Temp = LowFloorEntry.Value.Span(Temp);
                    }

                    KeyValuePair<Marker<T>, Range<T>> HighFloorEntry = this.LowIndexedRanges.FloorEntry(Temp.High);

                    if (!HighFloorEntry.Equals(default(KeyValuePair<Marker<T>, Range<T>>)) && HighFloorEntry.Value.Overlaps(Temp))
                    {
                        Temp = HighFloorEntry.Value.Span(Temp);
                    }

                    // Merge with any adjacent ranges
                    if (!LowFloorEntry.Equals(default(KeyValuePair<Marker<T>, Range<T>>)) && LowFloorEntry.Value.High.IsAdjacent(Temp.Low))
                    {
                        Temp = LowFloorEntry.Value.Span(Temp);
                    }

                    KeyValuePair<Marker<T>, Range<T>> HighHigherEntry = this.LowIndexedRanges.HigherEntry(Temp.High);

                    if (!HighHigherEntry.Equals(default(KeyValuePair<Marker<T>, Range<T>>)) && HighHigherEntry.Value.Low.IsAdjacent(Temp.High))
                    {
                        Temp = HighHigherEntry.Value.Span(Temp);
                    }

                    SortedDictionary<Marker<T>, Range<T>> SubMap = this.LowIndexedRanges.SubMap(Temp.Low, true, Temp.High, true);

                    this.LowIndexedRanges.Add(Temp.Low, Temp);
                }
            }
        }

        private SortedRangeSet(IDictionary<Marker<T>, Range<T>> lowIndexedRanges)
        {
            this.LowIndexedRanges = new SortedDictionary<Marker<T>, Range<T>>(lowIndexedRanges) ?? throw new ArgumentNullException("lowIndexedRanges");
        }

        #endregion

        #region Public Static Methods

        public static SortedRangeSet<T> Create(Range<T> first, params Range<T>[] args)
        {
            return new SortedRangeSet<T>(first, args);
        }

        public static SortedRangeSet<T> All()
        {
            return Create(Range<T>.All());
        }

        public static SortedRangeSet<T> None()
        {
            return Create(null);
        }

        #endregion

        #region Public Methods

        public IEnumerator<Range<T>> GetEnumerator()
        {
            IEnumerable<Range<T>> Temp = this.LowIndexedRanges.Values;
            return Temp.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable<Range<T>> Temp = this.LowIndexedRanges.Values;
            return Temp.GetEnumerator();
        }

        #endregion
    }
}
