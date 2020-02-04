
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    public static class Extensions_Boolean
    {
        /// <summary>
        /// Returns the boolean value to a human readable Yes or No string value.
        /// </summary>
        public static string ToYesNo(this bool value)
        {
            string result = "No";

            if (value)
            {
                result = "Yes";
            }

            return result;
        }

        public static string Switch(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public static bool FromString(this bool value, string text)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(text))
                switch (text.ToUpperInvariant())
                {
                    case "TRUE":
                    case "T":
                    case "YES":
                    case "Y":
                    case "1":
                    case "-1":
                    case "ON":
                        result = true;
                        break;
                }
            return result;
        }
    }
}