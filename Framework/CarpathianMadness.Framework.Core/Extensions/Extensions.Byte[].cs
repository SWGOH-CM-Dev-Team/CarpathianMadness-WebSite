
using System;
using System.Text;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        public static string ToHexadecimalString(this byte[] instance)
        {
            return ToHexadecimalString(instance, false);
        }

        public static string ToHexadecimalString(this byte[] instance, bool upperCase)
        {
            const string UpperCaseFormat = "{0:X2}";
            const string LowerCaseFormat = "{0:x2}";

            if ((instance == null) || (instance.Length == 0))
                return null;

            var format = upperCase ? UpperCaseFormat : LowerCaseFormat;
            var builder = new StringBuilder((instance.Length * 2) + 8);

            for (var i = 0; i < instance.Length; i++)
            {
                builder.AppendFormat(format, instance[i]);
            }

            return builder.ToString();
        }

        public static string ToBase64(this byte[] instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return Convert.ToBase64String(instance);
        }
    }
}