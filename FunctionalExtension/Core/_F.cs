using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Unit Unit() => default;

        public static None None() => FunctionalExtension.None.Default;
        public static Some<T> Some<T>(T value) => new Some<T>(value);

        public static Func<R> Using<T, R>(this T disposable, Func<T, R> f) where T : IDisposable
        {
            using (disposable) return () => f(disposable);
        }

        public static Func<bool> Not(this Func<bool> predicate) => () => !predicate();

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) => ts.Select(f);
        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) => optT.Match(some => f(some), _ => None().AsOption<R>());
        public static Option<T> AsOption<T>(this Some<T> some) => some;
        public static Option<T> AsOption<T>(this None none) => none;
        public static IEnumerable<R> FlatMap<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f) => ts.SelectMany(f);
        public static Option<R> FlatMap<T, R>(this Option<T> optT, Func<T, R> f) => optT.Match(some => Some(f(some)), _ => None().AsOption<R>());
        public static Option<R> FlatMap<T, R>(this Some<T> optT, Func<T, R> f) => ((Option<T>)optT).FlatMap(f);
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> f) => ts.Where(f);
        public static Option<T> Filter<T>(this Option<T> ts, Func<T, bool> f) => ts.Match(some => f(some) ? Some(some.Value).AsOption() : None(), none => none);

        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action) => ts.Map(action.ToFunc()).Execute();
        public static IEnumerable<T> Execute<T>(this IEnumerable<T> ts) => ts.ToImmutableList();
        public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();
    }
}