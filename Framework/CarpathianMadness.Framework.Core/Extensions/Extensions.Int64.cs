
using System;
using System.Globalization;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        public static string ToInvariantString(this Int64 item)
        {
            return item.ToString(CultureInfo.InvariantCulture);
        }
    }
}
