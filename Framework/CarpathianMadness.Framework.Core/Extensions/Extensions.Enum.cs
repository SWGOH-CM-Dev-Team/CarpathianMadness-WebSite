
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        /// <summary>
        /// Includes an enumerated type and returns the new value
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static T Include<T>(this Enum value, T append)
        {
            if (value == null)
                throw new NullReferenceException();
            Type type = value.GetType();

            //determine the values
            object result = value;
            _EnumValue parsed = new _EnumValue(append, type);
            if (parsed.Signed is long)
            {
                result = Convert.ToInt64(value, CultureInfo.InvariantCulture) | (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                result = Convert.ToUInt64(value, CultureInfo.InvariantCulture) | (ulong)parsed.Unsigned;
            }

            //return the final value
            return (T)Enum.Parse(type, result.ToString());
        }

        /// <summary>
        /// Removes an enumerated type and returns the new value
        /// </summary>
        //public static T Remove<T>(this Enum value, T remove)
        //{
        //    Type type = value.GetType();

        //    //determine the values
        //    object result = value;
        //    _EnumValue parsed = new _EnumValue(remove, type);
        //    if (parsed.Signed is long)
        //    {
        //        result = Convert.ToInt64(value) & ~(long)parsed.Signed;
        //    }
        //    else if (parsed.Unsigned is ulong)
        //    {
        //        result = Convert.ToUInt64(value) & ~(ulong)parsed.Unsigned;
        //    }

        //    //return the final value
        //    return (T)Enum.Parse(type, result.ToString());
        //}

        /// <summary>
        /// Checks if an enumerated type contains a value
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool Has<T>(this Enum value, T check)
        {
            if (value == null)
                throw new NullReferenceException();
            Type type = value.GetType();

            //determine the values
            //object result = value;
            _EnumValue parsed = new _EnumValue(check, type);
            if (parsed.Signed is long)
            {
                return (Convert.ToInt64(value, CultureInfo.InvariantCulture) & (long)parsed.Signed) == (long)parsed.Signed;
            }
            else if (parsed.Unsigned is ulong)
            {
                return (Convert.ToUInt64(value, CultureInfo.InvariantCulture) & (ulong)parsed.Unsigned) == (ulong)parsed.Unsigned;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an enumerated type is missing a value
        /// </summary>
        public static bool Missing<T>(this Enum obj, T value)
        {
            return !Extensions.Has<T>(obj, value);
        }

        //class to simplfy narrowing values between 
        //a ulong and long since either value should
        //cover any lesser value
        private class _EnumValue
        {
            //cached comparisons for tye to use
            private static Type _UInt64 = typeof(ulong);
            private static Type _UInt32 = typeof(long);

            public long? Signed;
            public ulong? Unsigned;

            public _EnumValue(object value, Type type)
            {
                //make sure it is even an enum to work with
                if (!type.IsEnum)
                {
                    throw new ArgumentException("Value provided is not an enumerated type!");
                }

                //then check for the enumerated value
                Type compare = Enum.GetUnderlyingType(type);

                //if this is an unsigned long then the only
                //value that can hold it would be a ulong
                if (compare.Equals(_EnumValue._UInt32) || compare.Equals(_EnumValue._UInt64))
                {
                    this.Unsigned = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                }
                //otherwise, a long should cover anything else
                else
                {
                    this.Signed = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                }
            }
        }
    }
}