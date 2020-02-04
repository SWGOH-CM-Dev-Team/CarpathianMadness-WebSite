
using System;
using System.Collections.Concurrent;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> obj, TKey key, int attempts)
        {
            TValue value;
            for (int attempt = 0; attempt < attempts; attempt++)
                if (obj.ContainsKey(key))
                    return true;
                else
                    if (obj.TryRemove(key, out value))
                    return true;
            return false;
        }

        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> obj, TKey key)
        {
            const int DefaultAttempts = 2;
            return Remove(obj, key, DefaultAttempts);
        }
    }
}
