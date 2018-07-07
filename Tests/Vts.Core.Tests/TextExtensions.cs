using System;

namespace Vts.Core.Tests
{
    public static class TextExtensions
    {
        public static string RandStr(this string str)
        {
            return string.Format("{0}-{1}", str, Guid.NewGuid().ToString().Substring(0, 5));
        }
    }
}