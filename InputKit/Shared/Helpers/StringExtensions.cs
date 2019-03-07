using System;

namespace Plugin.InputKit.Shared.Helpers
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source?.IndexOf(value, comp) >= 0;
        }
    }
}
