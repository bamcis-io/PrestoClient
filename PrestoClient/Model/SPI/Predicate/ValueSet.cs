using BAMCIS.PrestoClient.Model.SPI.Type;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// A default implementation for the IValueSet interface static methods
    /// </summary>
    public static class ValueSet
    {
        public static IValueSet None(IType type)
        {
            return AllOrNoneValueSet.None(type);
        }

        public static IValueSet All(IType type)
        {
            return AllOrNoneValueSet.All(type);
        }

        public static IValueSet Of(IType type, object first, params object[] rest)
        {
            return SortedRangeSet.Of(type, first, rest);
        }

        public static IValueSet CopyOf(IType type, IEnumerable<object> values)
        {
            return SortedRangeSet.CopyOf(type, values.Select(x => Range.Equal(type, x)));
        }

        public static IValueSet OfRanges(Range first, params Range[] rest)
        {
            return SortedRangeSet.CopyOf(first.Type, rest);
        }

        public static IValueSet CopyOfRanges(IType type, IEnumerable<Range> ranges)
        {
            return SortedRangeSet.CopyOf(type, ranges);
        }
    }
}
