
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        public static bool IsSame<T>(this T target, T source)
        {
            return source.GetType()
                .GetProperties()
                .All(p =>
                {
                    var s = p.GetValue(source, null);
                    var t = p.GetValue(target, null);
                    if (s == null && t == null)
                    {
                        return true;
                    }
                    if (s == null && t == null)
                    {
                        return false;
                    }
                    var isSame = s.Equals(t);
                    return isSame;
                }
            );
        }
    }
}
