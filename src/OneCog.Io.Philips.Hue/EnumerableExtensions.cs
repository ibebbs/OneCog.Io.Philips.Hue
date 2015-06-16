using System;
using System.Collections.Generic;

namespace OneCog.Io.Philips.Hue
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        public static T FirstOrValue<T>(this IEnumerable<T> source, T value)
        {
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext() ? enumerator.Current : value;
            }
        }
    }
}
