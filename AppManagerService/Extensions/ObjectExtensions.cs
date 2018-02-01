using System;

using JetBrains.Annotations;

namespace AppManagerService.Extensions
{
    public static class ObjectExtensions
    {
        [NotNull]
        public static T ThrowIfNull<T>(this T obj, string message)
        {
            if (obj == null)
            {
                throw new Exception($"Check object for null failed: {message}");
            }

            return obj;
        }
    }
}