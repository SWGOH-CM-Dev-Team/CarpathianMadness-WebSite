using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    public static class Extensions_DataRow
    {
        #region Public Methods

        #region GetValueOrDefault

        public static TType GetValueOrDefault<TType>(this DataRow dataRow, int index)
        {
            return GetValueOrDefault<TType>(dataRow, index, default(TType));
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TType GetValueOrDefault<TType>(this DataRow dataRow, int index, TType defaultValue)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }

            TType result = defaultValue;

            if (!IsNullOrDBNull(dataRow, index))
            {
                result = (TType)dataRow[index];
            }

            return result;
        }

        public static TType GetValueOrDefault<TType>(this DataRow dataRow, string columnName)
        {
            return GetValueOrDefault<TType>(dataRow, columnName, default(TType));
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TType GetValueOrDefault<TType>(this DataRow dataRow, string columnName, TType defaultValue)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }

            TType result = defaultValue;

            if (!IsNullOrDBNull(dataRow, columnName))
            {
                result = (TType)dataRow[columnName];
            }

            return result;
        }

        #endregion GetValueOrDefault

        #region GetValue

        public static DateTime GetDateTime(this DataRow dataRow, int index)
        {
            return  dataRow.Field<DateTime>(index);
        }

        public static DateTime GetDateTime(this DataRow dataRow, string columnName)
        {
            return dataRow.Field<DateTime>(columnName);
        }

        public static decimal GetDecimal(this DataRow dataRow, int index)
        {
            return dataRow.Field<decimal>(index);
        }

        public static decimal GetDecimal(this DataRow dataRow, string columnName)
        {
            return dataRow.Field<decimal>(columnName);
        }

        #endregion GetValue

        /// <summary>
        /// Checks if the value residing within the dataRow using the provided index
        /// is null or the value equals DBNull.Value.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool IsNullOrDBNull(this DataRow dataRow, int index)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var rowValue = dataRow[index];

            return (rowValue == null) || rowValue.Equals(DBNull.Value);
        }

        /// <summary>
        /// Checks if the value residing within the dataRow using the provided column name.
        /// is null or the value equals DBNull.Value.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool IsNullOrDBNull(this DataRow dataRow, string columnName)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException("dataRow");
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentStringException("columnName");
            }

            var rowValue = dataRow[columnName];

            return (rowValue == null) || rowValue.Equals(DBNull.Value);
        }

        #endregion Public Methods
    }
}
