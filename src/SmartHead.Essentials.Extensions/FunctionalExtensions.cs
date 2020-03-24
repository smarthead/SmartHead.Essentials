using System;
using System.Collections.Generic;

namespace SmartHead.Essentials.Extensions
{
    public static class FunctionalExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> x)
        {
            foreach (var e in enumerable)
                x(e);
        }
    }
}
