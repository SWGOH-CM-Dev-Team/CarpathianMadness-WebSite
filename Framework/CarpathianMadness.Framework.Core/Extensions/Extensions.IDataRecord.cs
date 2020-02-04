
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        public static TValue GetValue<TValue>(this IDataRecord reader, string columnName)
        {
            return GetValueOrDefault<TValue>(reader, columnName, default(TValue));
        }

        public static TValue GetValueOrDefault<TValue>(this IDataRecord reader, string columnName)
        {
            return GetValueOrDefault<TValue>(reader, columnName, default(TValue));
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TValue GetValueOrDefault<TValue>(this IDataRecord reader, string columnName, TValue defaultValue)
        {
            if (reader == null)
                throw new NullReferenceException();
            return ProcessRowValue(reader[columnName], defaultValue);
        }

        public static TValue GetValue<TValue>(this IDataRecord reader, int index)
        {
            return GetValueOrDefault<TValue>(reader, index, default(TValue));
        }

        public static TValue GetValueOrDefault<TValue>(this IDataRecord reader, int index)
        {
            return GetValueOrDefault<TValue>(reader, index, default(TValue));
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static TValue GetValueOrDefault<TValue>(this IDataRecord reader, int index, TValue defaultValue)
        {
            if (reader == null)
                throw new NullReferenceException();
            return ProcessRowValue(reader[index], defaultValue);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool ColumnExists(this IDataRecord reader, string columnName)
        {
            if (reader == null)
                throw new NullReferenceException();
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException("columnName");
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.GetName(i) == columnName)
                    return true;
            return false;
        }

        #endregion Public Methods

        #region Private Methods

        private static TValue ProcessRowValue<TValue>(object rowValue, TValue defaultValue)
        {
            TValue result = defaultValue;

            if (!rowValue.Equals(DBNull.Value))
            {
                result = (TValue)rowValue;
            }

            return result;
        }

        #endregion Private Methods
    }
}
