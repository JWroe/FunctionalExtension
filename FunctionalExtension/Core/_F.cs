using System;
using Unit = System.ValueTuple;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Func<R> Using<T, R>(this T disposable, Func<T, R> f) where T : IDisposable
        {
            using (disposable) return () => f(disposable);
        }

        public static Func<bool> Not(this Func<bool> predicate) => () => !predicate();
        public static Unit Unit() => default;

        public static None None() => FunctionalExtension.None.Default;
        public static Some<T> Some<T>(T value) => new Some<T>(value);

        public static Option<T> AsOption<T>(this Some<T> some) => some;
        public static Option<T> AsOption<T>(this None none) => none;
        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) => optT.Match(some => f(some), _ => None().AsOption<R>());
        public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f) => optT.Match(some => f(some), _ => None().AsOption<R>());

    }
}