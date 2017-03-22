using System;

namespace YoApp.Utils.Extensions
{
    public static class Experimental
    {
        public static T ChainIf<T>(this T target, bool condition, Action<T> next) where T : class
        {
            if (condition)
                next.Invoke(target);

            return target;
        }
    }
}
