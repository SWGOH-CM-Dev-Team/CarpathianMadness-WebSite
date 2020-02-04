
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarpathianMadness.Framework
{
    public static class Extensions_ICollection
    {
        /// <summary>
        /// Tries to obtain the value from the provided collection of values that resides at the provided index.
        /// </summary>
        /// <returns>Value found at the provided index, otherwise null for reference types or default value for value types.</returns>
        public static TObject TryGet<TObject>(this ICollection<TObject> values, int index)
        {
            return TryGet(values, index, default(TObject));
        }

        /// <summary>
        /// Tries to obtain the value from the provided collection of values that resides at the provided index.
        /// </summary>
        /// <returns>Value found at the provided index, otherwise the provided defaultValue.</returns>
        public static TObject TryGet<TObject>(this ICollection<TObject> values, int index, TObject defaultValue)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (index < 0)
                throw new ArgumentException("index cannot be less than zero.");

            var result = defaultValue;
            if (values.Count > index)
                result = values.ElementAt(index);
            return result;
        }

        public static void ForEach<T>(this ICollection<T> instance, Action<T> action)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (instance.Count == 0)
            {
                return;
            }

            foreach (var item in instance)
            {
                action(item);
            }
        }
    }
}