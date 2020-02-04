
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework
{
    public static partial class TryCast
    {
        #region Public Methods

        /// <summary>
        /// Attempts to the convert the provided object value to the desired CastType using the given provider.
        /// If the value cannot be cast to the desired type than it will return the given default value.
        /// </summary>
        public static CastType AsGeneric<CastType>(object value, IFormatProvider provider, CastType defaultValue)
        {
            CastType result = defaultValue;

            try
            {
                if (value != null)
                {
                    result = (CastType)Convert.ChangeType(value, typeof(CastType), provider);
                }
            }
            catch
            {
                //Do nothing...
            }

            return result;
        }

        /// <summary>
        /// Attempts to the convert the provided object value to the desired nullable CastType using the given provider.
        /// If the value cannot be cast to the desired type than it will return the given default value.
        /// </summary>
        public static Nullable<CastType> AsNullable<CastType>(object value, IFormatProvider provider, Nullable<CastType> defaultValue) where CastType : struct
        {
            Nullable<CastType> result = defaultValue;

            try
            {
                if (value != null)
                {
                    result = (Nullable<CastType>)Convert.ChangeType(value, typeof(CastType), provider);
                }
            }
            catch
            {
                //Do nothing...
            }

            return result;
        }

        #endregion Public Methods
    }
}
