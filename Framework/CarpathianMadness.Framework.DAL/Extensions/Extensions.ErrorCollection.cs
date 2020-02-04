
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpathianMadness.Framework.DAL
{
    public static partial class Extensions
    {
        #region Public Methods

        public static void AddZeroOrLess(this ErrorCollection instance, string fieldName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("fieldName");
            }

            instance.AddValue(fieldName + " cannot be zero or less.");
        }

        public static void AddInvalidEnum(this ErrorCollection instance, string fieldName, Enum enumValue)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("fieldName");
            }

            instance.AddValue(fieldName + " cannot be set '" + enumValue.ToString() + "'.");
        }

        public static void AddInvalidString(this ErrorCollection instance, string fieldName, int length = 0)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("fieldName");
            }

            if (length > 0)
            {
                instance.AddValue(fieldName + " cannot be null, empty, whitespace or greater than " + length + " characters.");
            }
            else
            {
                instance.AddValue(fieldName + " cannot be null, empty or whitespace.");
            }
        }

        #endregion Public Methods
    }
}