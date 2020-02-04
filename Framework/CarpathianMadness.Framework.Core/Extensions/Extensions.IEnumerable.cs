
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework
{
    public static class Extensions_IEnumerable
    {
        public static int Product(this IEnumerable<int> items)
        {
            return items.Aggregate((product, next) => product * next);
        }

        public static long Product(this IEnumerable<long> items)
        {
            return items.Aggregate((product, next) => product * next);
        }

        public static float Product(this IEnumerable<float> items)
        {
            return items.Aggregate((product, next) => product * next);
        }

        public static double Product(this IEnumerable<double> items)
        {
            return items.Aggregate((product, next) => product * next);
        }

        public static decimal Product(this IEnumerable<decimal> items)
        {
            return items.Aggregate((product, next) => product * next);
        }

        public static double Variance(this IEnumerable<double> items)
        {
            var result = 0d;
            var mean = items.Average();
            var count = (int)0;
            foreach (var value in items)
            {
                result += ((value - mean) * (value - mean));
                count++;
            }
            return result / (count - 1);
        }

        public static double VarianceP(this IEnumerable<double> items)
        {
            var result = 0d;
            var mean = items.Average();
            var count = (int)0;
            foreach (var value in items)
            {
                result += ((value - mean) * (value - mean));
                count++;
            }
            return result / count;
        }

        public static IEnumerable<TDestination> ConvertTo<TSource, TDestination>(this IEnumerable<TSource> items, Func<TSource, TDestination> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");

            var result = new List<TDestination>();

            foreach (var item in items)
                result.Add(func(item));

            return result;
        }

        public static string Join<T>(this IEnumerable<T> instance, string seperator)
        {
            return Join<T>(instance, seperator, (x) => { return x.ToString(); });
        }

        public static string Join<T>(this IEnumerable<T> instance, string seperator, Func<T, string> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");
            if (seperator == null)
                seperator = string.Empty;

            var builder = new StringBuilder();

            foreach (var item in instance)
            {
                if (item == null)
                    continue;

                builder.Append(func(item));
                builder.Append(seperator);
            }

            if (builder.Length > 0 && seperator.Length > 0)
                builder.Remove(builder.Length - seperator.Length, seperator.Length);

            return builder.ToString();
        }
    }
}