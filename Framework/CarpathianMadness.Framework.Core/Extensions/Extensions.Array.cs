
using System;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public static partial class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Tries to obtain the value from the provided array of values that resides at the provided index.
        /// </summary>
        /// <returns>Value found at the provided index, otherwise null for reference types or default value for value types.</returns>
        public static ArrayValueType TryGet<ArrayValueType>(this ArrayValueType[] values, int index)
        {
            return TryGet<ArrayValueType>(values, index, default(ArrayValueType));
        }

        /// <summary>
        /// Tries to obtain the value from the provided array of values that resides at the provided index.
        /// </summary>
        /// <returns>Value found at the provided index, otherwise the provided defaultValue.</returns>
        public static ArrayValueType TryGet<ArrayValueType>(this ArrayValueType[] values, int index, ArrayValueType defaultValue)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (index < 0)
                throw new ArgumentException("index cannot be less than zero.");
            return (values.Length > index) ? values[index] : defaultValue;
        }

        /// <summary>
        /// Takes the provided array and copies it, increasing its length by one and appending the provided item in the highest index position.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existingItems">The existing array</param>
        /// <param name="itemToAppend">The item to append</param>
        /// <returns>A new array of type T</returns>
        public static T[] Append<T>(this T[] existingItems, T itemToAppend)
        {
            var result = (T[])null;

            if ((existingItems == null) || (existingItems.Length == 0))
                return new T[] { itemToAppend };

            result = new T[existingItems.Length + 1];

            for (var index = 0; index < existingItems.Length; index++)
                result[index] = existingItems[index];

            result[result.Length - 1] = itemToAppend;

            return result;
        }

        #endregion Public Methods
    }
}
