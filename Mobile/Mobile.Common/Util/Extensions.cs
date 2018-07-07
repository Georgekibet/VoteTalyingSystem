using System;
using System.Collections.Generic;

namespace Mobile.Common.Util
{
    public static class Extensions
    {
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i]);
            }
        }
    }
}