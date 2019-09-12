using System;
using FunctionalExtension.Option;
using Unit = System.ValueTuple;

namespace FunctionalExtension
{
    public static class F
    {
        public static Func<R> Using<T, R>(this T disposable, Func<T, R> f) where T : IDisposable
        {
            using (disposable) return () => f(disposable);
        }

        public static Func<bool> Not(this Func<bool> predicate) => () => !predicate();
        public static Unit Unit() => default;

        public static None None => None.Default;
        public static Some<T> Some<T>(T value) => new Some<T>(value);
    }
}