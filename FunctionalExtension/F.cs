using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Unit = System.ValueTuple;

namespace FunctionalExtension
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
        public static Option<T> AsOption<T>([NotNull] this None none) => none;
        public static Some<T> Some<T>([NotNull] T value) => new Some<T>(value);
        public static Option<T> AsOption<T>([NotNull] this Some<T> some) => some;

        public static Option<int> Parse(this string str) => int.TryParse(str, out var num) ? Some(num).AsOption() : None();
    }
}