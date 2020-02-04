
using System;

namespace CarpathianMadness.Framework
{
    public static class Extensions_TimeSpan
    {
        /// <summary>
        /// Gets the total years of this instance.
        /// </summary>
        public static double GetYears(this TimeSpan span)
        {
            return Math.Abs(span.TotalDays / 365.25);
        }
    }
}