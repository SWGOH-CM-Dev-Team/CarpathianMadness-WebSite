
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        public static decimal ComputeDecimal(this DataTable table, string expression)
        {
            return Compute<decimal>(table, expression, string.Empty);
        }

        public static decimal ComputeDecimal(this DataTable table, string expression, string filter)
        {
            return Compute<decimal>(table, expression, filter);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TType Compute<TType>(this DataTable table, string expression)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            return (TType)table.Compute(expression, string.Empty);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TType Compute<TType>(this DataTable table, string expression, string filter)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            return (TType)table.Compute(expression, filter);
        }
    }
}
