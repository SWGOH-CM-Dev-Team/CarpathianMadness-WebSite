
using System;
using System.Collections.Generic;

namespace CarpathianMadness.Framework
{
    public static class Extensions_IDictionary
    {
        /// <summary>
        /// Attempts to add the provided key and value pair, if the key already exists then nothing is added.
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value)
        {
            bool outcome = false;

            if (instance != null && !instance.ContainsKey(key))
            {
                instance.Add(key, value);
                outcome = true;
            }

            return outcome;
        }

        /// <summary>
        /// Attempts to update the value which is paired with the provided key, if the key does not exist then nothing is updated.
        /// </summary>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value)
        {
            bool outcome = false;

            if (instance != null && instance.ContainsKey(key))
            {
                instance[key] = value;
                outcome = true;
            }

            return outcome;
        }

        /// <summary>
        /// Updates the provided key/value pair if it exists, if not then it will add the key/value pair to the dictionary.
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (instance.ContainsKey(key))
            {
                instance[key] = value;
            }
            else
            {
                instance.Add(key, value);
            }
        }

        /// <summary>
        /// Adds the provided key/value pair if it exists, if not then it will add the key/value pair to the dictionary.
        /// </summary>
        [Obsolete("Duplicate code. Use '.AddOrUpdate<>' method instead.")]
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value)
        {
            instance.AddOrUpdate(key, value);
        }

        /// <summary>
        /// Attempts to retrieve the value matching the provided key, otherwise will
        /// return a default value (null for reference types or minimum values for value types).
        /// </summary>
        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key)
        {
            return TryGet<TKey, TValue>(instance, key, default(TValue));
        }

        /// <summary>
        /// Attempts to retrieve the value matching the provided key, otherwise will
        /// return the provided defaultValue.
        /// </summary>
        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue defaultValue)
        {
            TValue result = defaultValue;

            if (instance != null && instance.ContainsKey(key))
            {
                result = instance[key];
            }

            return result;
        }

        /// <summary>
        /// Checks if all keys exist within the provided IDictionary instance.
        /// </summary>
        /// <returns><code>True</code> if all keys exist, otherwise <code>False</code>.</returns>
        public static bool ContainsAllKeys<TKey, TValue>(this IDictionary<TKey, TValue> instance, params TKey[] keys)
        {
            bool result = false;

            if (instance != null && instance.Count > 0 && keys != null && keys.Length > 0)
            {
                int foundCount = 0;

                for (int i = 0; i < keys.Length; i++)
                {
                    TKey key = keys[i];

                    if (instance.ContainsKey(key))
                    {
                        foundCount++;
                    }
                }

                result = foundCount == keys.Length;
            }
            return result;
        }


        public static bool ContainsInvariantKey<TValue>(this IDictionary<string, TValue> instance, string key)
        {
            var invariantKey = key.ToUpperInvariant();
            foreach (var instanceKey in instance.Keys)
                if (instanceKey.ToUpperInvariant() == invariantKey)
                    return true;
            return false;
        }

        public static TValue GetByInvariantKey<TValue>(this IDictionary<string, TValue> instance, string key)
        {
            var invariantKey = key.ToUpperInvariant();
            foreach (var instanceKey in instance.Keys)
                if (instanceKey.ToUpperInvariant() == invariantKey)
                    return instance[instanceKey];
            throw new KeyNotFoundException();
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> instance, string key, out T newVal)
        {

            object val;
            var rtn = instance.TryGetValue(key, out val);

            newVal = default(T);

            if (rtn)
                newVal = (T)Convert.ChangeType(val, typeof(T));

            return rtn;
        }

        public static bool TryGetValue<T>(this IDictionary<ushort, object> instance, ushort key, out T newVal)
        {

            object val;
            var rtn = instance.TryGetValue(key, out val);

            newVal = default(T);

            if (rtn)
                newVal = (T)Convert.ChangeType(val, typeof(T));

            return rtn;
        }
    }
}