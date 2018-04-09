using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI
{
    public sealed class Domain<T>
    {
        #region Public Properties

        public SortedRangeSet<T> Ranges { get; }

        public bool NullAllowed { get; }

        #endregion

        #region Constructors

        public Domain(SortedRangeSet<T> ranges, bool nullAllowed)
        {
            this.Ranges = ranges ?? throw new ArgumentNullException("ranges");
            this.NullAllowed = nullAllowed;
        }

        #endregion

        #region Public Methods

        public bool IsNone()
        {
            return this.Equals(Domain<T>.None());
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

            Domain<T> Other = (Domain<T>)obj;

            return this.Ranges.Equals(Other.Ranges) &&
                    this.NullAllowed.Equals(Other.NullAllowed);
        }

        public bool IsAll()
        {
            return this.Equals(Domain<T>.All());
        }


        #endregion

        #region Public Static Methods

        public static Domain<T> Create(SortedRangeSet<T> ranges, bool nullAllowed)
        {
            return new Domain<T>(ranges, nullAllowed);
        }

        public static Domain<T> None()
        {
            return new Domain<T>(SortedRangeSet<T>.None(), false);
        }

        public static Domain<T> All()
        {
            return new Domain<T>(SortedRangeSet<T>.Create(Range<T>.All()), true);
        }

        public static Domain<T> OnlyNull()
        {
            return new Domain<T>(SortedRangeSet<T>.None(), true);
        }

        public static Domain<T> NotNull()
        {
            return new Domain<T>(SortedRangeSet<T>.All(), false);
        }

        public static Domain<T> SingleValue(IComparable<T> value)
        {
            return new Domain<T>(new SortedRangeSet<T>(Range<T>.Equal(value)), false);
        }

        #endregion
    }
}
