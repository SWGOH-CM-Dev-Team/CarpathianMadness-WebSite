
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool HasAttribute<TAttribute>(this object item) where TAttribute : Attribute
        {
            if (item == null)
                throw new NullReferenceException();

            var attributes = Attribute.GetCustomAttributes(item.GetType());
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                    if (attribute is TAttribute)
                        return true;
            }
            return false;
        }

        #endregion Public Methods
    }
}