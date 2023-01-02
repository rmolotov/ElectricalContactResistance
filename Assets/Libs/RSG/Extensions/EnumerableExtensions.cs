using System;
using System.Collections.Generic;

namespace RSG.Extensions
{
    /// <summary>
    /// General extensions to LINQ.
    /// </summary>
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> source, Action<T> fn)
        {
            foreach (var item in source) 
                fn.Invoke(item);
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T, int> fn)
        {
            var index = 0;

            foreach (var item in source)
            {
                fn.Invoke(item, index);
                index++;
            }
        }
    }
}
