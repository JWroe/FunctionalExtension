using System;

namespace FunctionalExtension
{
    public static partial class F 
    {
        public static T Compose<T>(this T value, Func<T, T> first, Func<T, T> second) => second(first(value));
    }
}