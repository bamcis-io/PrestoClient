using BAMCIS.PrestoClient.Model.SPI.Type;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.ValueSet.java
    /// </summary>
    public interface IValueSet
    {
        /* 
         * These are implemented in ValueSet since they are static

        // static IValueSet None(string type);

        // static IValueSet All(string type);

        // static IValueSet Of(string type, object first, params object[] rest);

        // static IValueSet CopyOf(string type, IEnumerable<object> values);

        // static IValueSet OfRanges(Range first, params Range[] rest);

        // static IValueSet CopyOfRanges(string type, IEnumerable<Range> ranges);

        */

        IType GetPrestoType();

        bool IsNone();

        bool IsAll();

        bool IsSingleValue();

        object GetSingleValue();

        bool ContainsValue(object value);

        IValuesProcessor GetValuesProcessor();

        IValueSet Intersect(IValueSet other);

        IValueSet Union(IValueSet other);

        IValueSet Union(IEnumerable<IValueSet> valueSets);

        IValueSet Complement();

        bool Overlaps(IValueSet other);

        bool Contains(IValueSet other);

        IValueSet Subtract(IValueSet other);

        string ToString(IConnectorSession session);
    }
}
