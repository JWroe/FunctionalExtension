using System;

namespace FunctionalExtension.Extensions
{
    public static class FunctionExtensions
    {
        public static T Compose<T>(this T value, Func<T, T> first, Func<T, T> second) => second(first(value));
    }
}