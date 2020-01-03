using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace FunctionalExtension
{
    public static partial class F
    {
        public static Unit Unit() => default;

        public static None None() => FunctionalExtension.None.Default;
        public static Some<T> Some<T>(T value) => new Some<T>(value);

        public static Func<R> Using<T, R>(this T disposable, Func<T, R> f) where T : IDisposable
        {
            using (disposable)
            {
                return () => f(disposable);
            }
        }

        public static Func<bool> Not(this Func<bool> predicate) => () => !predicate();

        public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();

        public static IEnumerable<int> Range(int count = 1) => Enumerable.Range(0, count);

        private static char Sign(this in decimal p0) => p0 > 0 ? '+' : '-';
    }
}