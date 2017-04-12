using System;

namespace YoApp.Core.Extensions
{
    public static class ExperimentalExtensions
    {
        public static T ChainIf<T>(this T target, bool condition, Action<T> next) where T : class
        {
            if (condition)
                next?.Invoke(target);

            return target;
        }
    }
}
