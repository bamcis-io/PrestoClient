using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model
{
    public static class SortedDictionaryExtensions
    {
        private static Tuple<int, int> GetPossibleIndices<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary, TKey key, bool strictlyDifferent, out List<TKey> list)
        {
            list = dictionary.Keys.ToList();
            int index = list.BinarySearch(key, dictionary.Comparer);
            if (index >= 0)
            {
                // exists
                if (strictlyDifferent)
                    return Tuple.Create(index - 1, index + 1);
                else
                    return Tuple.Create(index, index);
            }
            else
            {
                // doesn't exist
                int indexOfBiggerNeighbour = ~index; //bitwise complement of the return value

                if (indexOfBiggerNeighbour == list.Count)
                {
                    // bigger than all elements
                    return Tuple.Create(list.Count - 1, list.Count);
                }
                else if (indexOfBiggerNeighbour == 0)
                {
                    // smaller than all elements
                    return Tuple.Create(-1, 0);
                }
                else
                {
                    // Between 2 elements
                    int indexOfSmallerNeighbour = indexOfBiggerNeighbour - 1;
                    return Tuple.Create(indexOfSmallerNeighbour, indexOfBiggerNeighbour);
                }
            }
        }

        public static TKey LowerKey<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, true, out list);
            if (indices.Item1 < 0)
                return default(TKey);

            return list[indices.Item1];
        }
        public static KeyValuePair<TKey, TValue> LowerEntry<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, true, out list);
            if (indices.Item1 < 0)
                return default(KeyValuePair<TKey, TValue>);

            var newKey = list[indices.Item1];
            return new KeyValuePair<TKey, TValue>(newKey, dictionary[newKey]);
        }

        public static TKey FloorKey<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, false, out list);
            if (indices.Item1 < 0)
                return default(TKey);

            return list[indices.Item1];
        }
        public static KeyValuePair<TKey, TValue> FloorEntry<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            Tuple<int, int> Indices = GetPossibleIndices(dictionary, key, false, out List<TKey> List);

            if (Indices.Item1 < 0)
            {
                return default(KeyValuePair<TKey, TValue>);
            }

            TKey NewKey = List[Indices.Item1];
            return new KeyValuePair<TKey, TValue>(NewKey, dictionary[NewKey]);
        }

        public static TKey CeilingKey<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, false, out list);
            if (indices.Item2 == list.Count)
                return default(TKey);

            return list[indices.Item2];
        }
        public static KeyValuePair<TKey, TValue> CeilingEntry<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, false, out list);
            if (indices.Item2 == list.Count)
                return default(KeyValuePair<TKey, TValue>);

            var newKey = list[indices.Item2];
            return new KeyValuePair<TKey, TValue>(newKey, dictionary[newKey]);
        }

        public static TKey HigherKey<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, true, out list);
            if (indices.Item2 == list.Count)
                return default(TKey);

            return list[indices.Item2];
        }
        public static KeyValuePair<TKey, TValue> HigherEntry<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, true, out list);
            if (indices.Item2 == list.Count)
                return default(KeyValuePair<TKey, TValue>);

            var newKey = list[indices.Item2];
            return new KeyValuePair<TKey, TValue>(newKey, dictionary[newKey]);
        }

        public static SortedDictionary<TKey, TValue> SubMap<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey fromKey, bool fromInclusive, TKey toKey, bool toInclusive)
        {
            IComparer<TKey> Comparer = dictionary.Comparer;

            return new SortedDictionary<TKey, TValue>(dictionary.Where(x => (fromInclusive ? Comparer.Compare(x.Key, fromKey) >= 0 : Comparer.Compare(x.Key, fromKey) > 0) &&
                toInclusive ? Comparer.Compare(x.Key, toKey) <= 0 : Comparer.Compare(x.Key, toKey) < 0).ToDictionary(x => x.Key, x => x.Value), Comparer);
        }
    }
}
