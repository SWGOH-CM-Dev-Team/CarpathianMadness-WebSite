
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        public static string ToStringOrDefault<T>(this Nullable<T> instance) where T : struct
        {
            if (instance == null)
            {
                return null;
            }

            if (!instance.HasValue)
            {
                return null;
            }

            return instance.Value.ToString();
        }

        #endregion Public Methods
    }
}