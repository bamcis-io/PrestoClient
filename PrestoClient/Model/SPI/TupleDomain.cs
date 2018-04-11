using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.TupleDomain.java
    /// Defines a set of valid tuples according to the constraints on each of its constituent columns.
    /// TODO: In progress
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TupleDomain<T>
    {
        #region Private Fields

        private static readonly TupleDomain<T> NONE = new TupleDomain<T>(null);

        private static readonly TupleDomain<T> ALL = new TupleDomain<T>(new Dictionary<IColumnHandle, Domain<T>>());

        #endregion

        #region Public Properties

        /**
         * TupleDomain is internally represented as a normalized map of each column to its
         * respective allowable value Domain. Conceptually, these Domains can be thought of
         * as being AND'ed together to form the representative predicate.
         *
         * This map is normalized in the following ways:
         * 1) The map will not contain Domain.none() as any of its values. If any of the Domain
         * values are Domain.none(), then the whole map will instead be null. This enforces the fact that
         * any single Domain.none() value effectively turns this TupleDomain into "none" as well.
         * 2) The map will not contain Domain.all() as any of its values. Our convention here is that
         * any unmentioned column is equivalent to having Domain.all(). To normalize this structure,
         * we remove any Domain.all() values from the map.
         */
        public IReadOnlyDictionary<IColumnHandle, Domain<T>> Domains { get; }

        #endregion

        #region Constructors

        private TupleDomain(IDictionary<IColumnHandle, Domain<T>> domains)
        {
            if (domains == null || ContainsNoneDomain(domains))
            {
                this.Domains = null;
            }
            else
            {
                this.Domains = NormalizeAndCopy(domains);
            }
        }

        #endregion

        #region Public Static Methods

        public static TupleDomain<T> WithColumnDomains(IDictionary<IColumnHandle, Domain<T>> domains)
        {
            return new TupleDomain<T>(domains);
        }

        public static TupleDomain<T> None()
        {
            return NONE;
        }

        public static TupleDomain<T> All()
        {
            return ALL;
        }

        /// <summary>
        /// Convert a map of columns to values into the TupleDomain which requires
        /// those columns to be fixed to those values.
        /// </summary>
        /// <param name="fixedValues"></param>
        /// <returns></returns>
        public static TupleDomain<T> WithFixedValues(IDictionary<IColumnHandle, IComparable<T>> fixedValues)
        {
            IDictionary<IColumnHandle, Domain<T>> Domains = new Dictionary<IColumnHandle, Domain<T>>();
            foreach (KeyValuePair<IColumnHandle, IComparable<T>> Entry in fixedValues)
            {
                Domains.Add(Entry.Key, Domain<T>.SingleValue(Entry.Value));
            }

            return WithColumnDomains(Domains);
        }

        public static TupleDomain<T> FromNullableColumnDomains(IEnumerable<ColumnDomain<T>> nullableColumnDomains)
        {
            if (nullableColumnDomains == null)
            {
                return None();
            }

            return WithColumnDomains(nullableColumnDomains.ToDictionary(x => x.ColumnHandle, x => x.Domain));
        }

        public bool IsAll()
        {
            return this.Domains != null && this.Domains.Count == 0;
        }

        public bool IsNone()
        {
            return this.Domains == null;
        }

        #endregion


        #region Private Methods

        private static bool ContainsNoneDomain(IDictionary<IColumnHandle, Domain<T>> domains)
        {
            foreach (KeyValuePair<IColumnHandle, Domain<T>> Domain in domains)
            {
                if (Domain.Value.IsNone())
                {
                    return true;
                }
            }

            return false;
        }

        private static IReadOnlyDictionary<IColumnHandle, Domain<T>> NormalizeAndCopy(IDictionary<IColumnHandle, Domain<T>> domains)
        {
            Dictionary<IColumnHandle, Domain<T>> Map = new Dictionary<IColumnHandle, Domain<T>>();

            foreach (KeyValuePair<IColumnHandle, Domain<T>> Entry in domains)
            {
                if (!Entry.Value.IsAll())
                {
                    Map.Add(Entry.Key, Entry.Value);
                }
            }

            return Map;
        }

        #endregion
    }
}
