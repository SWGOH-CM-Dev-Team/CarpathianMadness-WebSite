
using System;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static T Limit<T>(this IComparable<T> item, T lower, T upper)
        {
            if (item == null)
                throw new NullReferenceException();
            if (item.CompareTo(lower) < 0)
                return lower;
            if (item.CompareTo(upper) > 0)
                return upper;
            return (T)item;
        }
    }
}
