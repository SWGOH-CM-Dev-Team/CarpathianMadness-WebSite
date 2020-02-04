
using System;
using System.Globalization;

namespace CarpathianMadness.Framework
{
    public static class Extensions_Type
    {
        public static string GetReadableName(this Type type)
        {
            var typeName = type.Name;
            try
            {
                if (type.IsGenericType)
                {
                    var lhs = type.FullName.Split('`')[0];
                    var rhs = type.FullName.Split('`')[1].Substring(3).Split(',')[0];
                    var temp = lhs.Split('.');
                    lhs = temp[temp.Length - 1];
                    temp = rhs.Split('.');
                    rhs = temp[temp.Length - 1];
                    typeName = string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", lhs, rhs);
                }
            }
            catch
            {
            }
            return typeName;
        }
    }
}