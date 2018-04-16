using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI.Predicate
{
    /// <summary>
    /// From com.facebook.presto.spi.predicate.TupleDomain.java
    /// Defines a set of valid tuples according to the constraints on each of its constituent columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(TupleDomainConverter<>))]
    public sealed class TupleDomain<T>
    {
        #region Private Fields

        private static readonly TupleDomain<T> NONE = new TupleDomain<T>(null);

        private static readonly TupleDomain<T> ALL = new TupleDomain<T>(new Dictionary<T, Domain>());

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
        public IReadOnlyDictionary<T, Domain> Domains { get; }

        public IEnumerable<ColumnDomain<T>> ColumnDomains
        {
            get
            {
                return this.Domains.Select(x => new ColumnDomain<T>(x.Key, x.Value));
            }
        }

        #endregion

        #region Constructors

        private TupleDomain(IDictionary<T, Domain> domains)
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

        public static TupleDomain<T> WithColumnDomains(IDictionary<T, Domain> domains)
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
        /// Extract all column constraints that require exactly one value or only null in their respective Domains.
        /// Returns an empty Optional if the Domain is none.
        /// </summary>
        /// <param name="tupleDomain"></param>
        /// <returns></returns>
        public static IDictionary<T, object> ExtractFixedValues(TupleDomain<T> tupleDomain)
        {
            if (tupleDomain.Domains == null)
            {
                return null;
            }

            return tupleDomain.Domains.Where(x => x.Value.IsNullableSingleValue()).ToDictionary(x => x.Key, x => (object)x.Value);
        }

        /// <summary>
        /// Convert a map of columns to values into the TupleDomain which requires
        /// those columns to be fixed to those values.
        /// </summary>
        /// <param name="fixedValues"></param>
        /// <returns></returns>
        public static TupleDomain<T> FromFixedValues(IDictionary<T, object> fixedValues)
        {
            throw new NotImplementedException("This method is not yet implemented");
        }

        public static TupleDomain<T> FromColumnDomains(IEnumerable<ColumnDomain<T>> columnDomains)
        {
            if (columnDomains == null || !columnDomains.Any())
            {
                return None();
            }

            return WithColumnDomains(columnDomains.ToDictionary(x => x.Column, x => x.Domain));
        }

        public static TupleDomain<T> ColumnWiseUnion(TupleDomain<T> first, TupleDomain<T> second, params TupleDomain<T>[] rest)
        {
            List<TupleDomain<T>> Domains = new List<TupleDomain<T>>();

            if (first != null)
            {
                Domains.Add(first);
            }

            if (second != null)
            {
                Domains.Add(second);
            }

            if (rest != null && rest.Length > 0)
            {
                Domains.AddRange(rest);
            }

            return ColumnWiseUnion(Domains);
        }

        /// <summary>
        /// Returns a TupleDomain in which corresponding column Domains are unioned together.
        /// 
        /// Note that this is NOT equivalent to a strict union as the final result may allow tuples
        /// that do not exist in either TupleDomain.
        /// 
        /// For example:
        /// 
        /// TupleDomain X: a => 1, b => 2
        /// TupleDomain Y: a => 2, b => 3
        /// Column-wise unioned TupleDomain: a = > 1 OR 2, b => 2 OR 3
        /// 
        /// In the above resulting TupleDomain, tuple (a => 1, b => 3) would be considered valid but would
        /// not be valid for either TupleDomain X or TupleDomain Y.
        /// However, this result is guaranteed to be a superset of the strict union.
        /// </summary>
        /// <param name="tupleDomains"></param>
        /// <returns></returns>
        public static TupleDomain<T> ColumnWiseUnion(IEnumerable<TupleDomain<T>> tupleDomains)
        {
            if (tupleDomains == null || !tupleDomains.Any())
            {
                throw new ArgumentException("Tupledomains must have at least one element.");
            }

            if (tupleDomains.Count() == 1)
            {
                return tupleDomains.First();
            }

            // gather all common columns
            HashSet<T> CommonColumns = new HashSet<T>();

            // first, find a non-none domain
            bool Found = false;

            IEnumerator<TupleDomain<T>> Iterator = tupleDomains.GetEnumerator();

            while (Iterator.MoveNext())
            {
                if (!Iterator.Current.IsNone())
                {
                    Found = true;
                    foreach (T Item in Iterator.Current.Domains.Keys)
                    {
                        CommonColumns.Add(Item);
                    }

                    break;
                }
            }

            if (!Found)
            {
                return None();
            }

            // then, get the common columns
            while (Iterator.MoveNext())
            {
                if (!Iterator.Current.IsNone())
                {
                    // Only retain the common columns
                    CommonColumns = new HashSet<T>(CommonColumns.Intersect(Iterator.Current.Domains.Keys));
                }
            }

            // group domains by column (only for common columns)
            IDictionary<T, IEnumerable<Domain>> DomainsByColumn = tupleDomains
                .SelectMany(x => x.Domains)                 // Flatten
                .GroupBy(x => x.Key)                        // Group by T column
                .Where(x => CommonColumns.Contains(x.Key))  // Only take the ones where T is in common columns
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value)); // Send to dictionary

            // finally, do the column-wise union
            return WithColumnDomains(
                DomainsByColumn
                .Select(x => new KeyValuePair<T, Domain>(x.Key, Domain.Union(x.Value)))
                .ToDictionary(x => x.Key, x => x.Value)
            );
        }

        #endregion

        #region Public Methods

        public bool IsAll()
        {
            return this.Domains != null && this.Domains.Count == 0;
        }

        public bool IsNone()
        {
            return this.Domains == null;
        }

        /// <summary>
        /// Returns the strict intersection of the TupleDomains.
        /// The resulting TupleDomain represents the set of tuples that would be valid
        /// in both TupleDomains.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public TupleDomain<T> Intersect(TupleDomain<T> other)
        {
            if (this.IsNone() || other.IsNone())
            {
                return None();
            }

            IDictionary<T, Domain> Intersected = this.Domains.ToDictionary(x => x.Key, x => x.Value);

            foreach (KeyValuePair<T, Domain> Item in other.Domains)
            {
                if (!Intersected.ContainsKey(Item.Key))
                {
                    Intersected.Add(Item.Key, Item.Value);
                }
                else
                {
                    Intersected[Item.Key] = Item.Value.Intersect(Intersected[Item.Key]);
                }
            }

            return WithColumnDomains(Intersected);
        }

        /// <summary>
        /// Returns true only if there exists a strict intersection between the TupleDomains.
        /// i.e. there exists some potential tuple that would be allowable in both TupleDomains.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(TupleDomain<T> other)
        {
            return !this.Intersect(other).IsNone();
        }

        /// <summary>
        /// Returns true only if the this TupleDomain contains all possible tuples that would be allowable by
        /// the other TupleDomain.
        /// </summary>
        /// <param name="domains"></param>
        /// <returns></returns>
        public bool Contains(TupleDomain<T> other)
        {
            return other.IsNone() || ColumnWiseUnion(this, other).Equals(this);
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

            TupleDomain<T> Other = (TupleDomain<T>)obj;

            return Object.Equals(this.Domains, Other.Domains);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.Domains);
        }

        public string ToString(IConnectorSession session)
        {
            StringBuilder Buffer = new StringBuilder("TupleDomain:");

            if (this.IsAll())
            {
                Buffer.Append("ALL");
            }
            else if (this.IsNone())
            {
                Buffer.Append("NONE");
            }
            else
            {
                Buffer.Append(this.Domains.ToDictionary(x => x.Key, x => x.Value.ToString(session)));
            }

            return Buffer.ToString();
        }

        public TupleDomain<U> Transform<U>(Func<T, U> function)
        {
            if (this.Domains == null)
            {
                return TupleDomain<U>.None();
            }

            IDictionary<U, Domain> Result = new Dictionary<U, Domain>();

            foreach (KeyValuePair<T, Domain> Entry in this.Domains)
            {
                U Key = function.Invoke(Entry.Key);

                if (Key == null)
                {
                    continue;
                }

                if (Result.ContainsKey(Key))
                {
                    throw new ArgumentException($"Every argument must have a unique mapping, {Key.ToString()} maps to {Entry.Value.ToString()} and {Result[Key].ToString()}.");
                }

                Result.Add(Key, Entry.Value);
            }

            return TupleDomain<U>.WithColumnDomains(Result);
        }

        public TupleDomain<T> Simplify()
        {
            if (this.IsNone())
            {
                return this;
            }

            IDictionary<T, Domain> Simplified = this.Domains.ToDictionary(x => x.Key, x => x.Value.Simplify());

            return WithColumnDomains(Simplified);
        }

        #endregion

        #region Private Methods

        private static bool ContainsNoneDomain(IDictionary<T, Domain> domains)
        {
            return domains.Any(x => x.Value.IsNone());
        }

        private static IReadOnlyDictionary<T, Domain> NormalizeAndCopy(IDictionary<T, Domain> domains)
        {
           return domains.Where(x => !x.Value.IsAll()).ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion
    }
}
