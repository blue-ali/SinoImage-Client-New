using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocScanner.LibCommon
{
    public static class ForEachExt
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (var item in source)
                action(item);
        }
    }
}