
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CarpathianMadness.Framework
{
    public static class Extensions_IList
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.IList<T>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static void AddRange<TItem>(this IList<TItem> list, IList<TItem> items)
        {
            if (list == null)
                throw new NullReferenceException();
            if (items == null)
                throw new ArgumentNullException("items");
            if ((items != null) && (items.Count > 0))
                for (int i = 0; i < items.Count; i++)
                    list.Add(items[i]);
        }

        /// <summary>
        /// Selects elements of the collection based upon the indicies supplied.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static IEnumerable<TItem> SelectByIndex<TItem>(this IList<TItem> items, IEnumerable<int> indicies)
        {
            if (items == null)
                throw new NullReferenceException();
            if (indicies == null)
                throw new ArgumentNullException("indicies");
            foreach (var index in indicies)
                yield return items[index];
        }

        /// <summary>
        /// Selects elements of the collection based upon the index range supplied.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static IList<TItem> Slice<TItem>(this IList<TItem> items, int start, int finish)
        {
            if (items == null)
                throw new NullReferenceException();
            return items.Skip(start).Take((finish - start) + 1).ToList();
        }

        /// <summary>
        /// Selects the first element of the collection and then moves it to the last position in the list.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TItem FirstAndMoveToLast<TItem>(this IList<TItem> items)
        {
            if (items == null)
                throw new NullReferenceException();

            TItem item = items.First();

            items.RemoveAt(0);
            items.Add(item);

            return item;
        }

    }
}