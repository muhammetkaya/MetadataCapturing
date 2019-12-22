using System;

namespace MetadataExtractor.Extensions
{
    public static class CommonExtensions
    {
        public static bool Assigned(this string obj)
        {
            return string.IsNullOrEmpty(obj);
        }

        public static bool NotAssigned(this string obj)
        {
            return !Assigned(obj);
        }
    }
}
