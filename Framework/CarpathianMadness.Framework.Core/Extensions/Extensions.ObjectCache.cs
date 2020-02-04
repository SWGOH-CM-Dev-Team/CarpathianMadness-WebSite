
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Constants

        private const string _slidingKeyPrefix = "SLIDING_";
        private static readonly object _placeholder = new object();

        #endregion Constants

        #region Public Methods

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool AddBothExpirations(this ObjectCache instance, string key, object value, DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration, CacheEntryRemovedCallback removedDelegate = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            string slidingKey = _slidingKeyPrefix + key;

            CacheItemPolicy slidingPolicy = new CacheItemPolicy
            {
                SlidingExpiration = slidingExpiration
            };

            instance.Set(slidingKey, _placeholder, slidingPolicy);

            CacheEntryChangeMonitor monitor = instance.CreateCacheEntryChangeMonitor(new string[]{
                slidingKey
            });

            CacheItemPolicy itemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = absoluteExpiration,
                RemovedCallback = (x) => {
                    if (removedDelegate == null)
                    {
                        return;
                    }

                    //HACK: ChangeMonitorChanged is given due to one of the depedent
                    //monitors for an item being removed due to expiry, capture this and re-send as expired.
                    if (x.RemovedReason == CacheEntryRemovedReason.ChangeMonitorChanged)
                    {
                        x = new CacheEntryRemovedArguments(x.Source, CacheEntryRemovedReason.Expired, x.CacheItem);
                    }

                    removedDelegate(x);
                }
            };

            itemPolicy.ChangeMonitors.Add(monitor);

            return instance.Add(key, value, itemPolicy);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static object GetWithBothExpirations(this ObjectCache instance, string key)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentStringException("key");
            }

            //INFO: This is to trigger activity on
            //the placeholder item, otherwise will expire.
            instance.Get(_slidingKeyPrefix + key);

            return instance.Get(key);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static object RemoveWithBothExpirations(this ObjectCache instance, string key)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentStringException("key");
            }

            object removedObj = instance.Remove(key);
            instance.Remove(_slidingKeyPrefix + key);

            return removedObj;
        }

        #endregion Public Methods
    }
}
