
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CarpathianMadness.Framework
{
    public static class Extensions_DateTime
    {
        /// <summary>
        /// Returns the ISO-8601 format for the DateTime object
        /// </summary>
        /// <returns></returns>
        /// <see cref="http://en.wikipedia.org/wiki/ISO_8601" />
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iso")]
        public static string ToIso8601(this DateTime value)
        {
            var result = string.Empty;

            result += value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            result += "T";
            result += value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

            if (value.Kind == DateTimeKind.Utc)
            {
                result += "Z";
            }
            else
            {
                var offset = (int)TimeZone.CurrentTimeZone.GetUtcOffset(value).TotalHours;
                if (offset > 0)
                    result += string.Format(CultureInfo.InvariantCulture, "+{0:D2}", offset);
                else if (offset < 0)
                    result += string.Format(CultureInfo.InvariantCulture, "-{0:D2}", offset);
                else
                    result += "Z";
            }
            return result;
        }
    }
}