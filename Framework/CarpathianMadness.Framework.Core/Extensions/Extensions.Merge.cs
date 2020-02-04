
using System;
using System.Linq;

namespace CarpathianMadness.Framework
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public sealed class MergableFieldAttribute : Attribute
    {

    }

    public static partial class Extensions
    {
        public static TItem Merge<TItem>(this TItem target, object source)
        {
            var targetType = target.GetType();
            source.GetType().GetProperties().ToList().ForEach(
                p =>
                {
                    var upper = Char.ToUpperInvariant(p.Name[0]) + p.Name.Substring(1);
                    var value = p.GetValue(source, null);
                    var targetProperty = targetType.GetProperty(upper);
                    if (targetProperty != null)
                    {
                        var hasMergeableField = Attribute.IsDefined(targetProperty, typeof(MergableFieldAttribute));
                        if ((value != null) && (p.PropertyType == targetProperty.PropertyType) && hasMergeableField)
                        {
                            targetProperty.SetValue(target, value, null);
                        }
                    }
                }
            );
            return target;
        }

    }
}
