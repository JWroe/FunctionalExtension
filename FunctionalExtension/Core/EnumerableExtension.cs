using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) => ts.Select(f);
        public static IEnumerable<R> FlatMap<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f) => ts.SelectMany(f);

        public static IEnumerable<R> FlatMap<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) => list.FlatMap(t => func(t).AsEnumerable());
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> f) => ts.Where(f);

        public static IEnumerable<ValueTuple> ForEach<T>(this IEnumerable<T> ts, Action<T> action) => ts.Map(action.ToFunc()).Execute();
        public static IEnumerable<T> Execute<T>(this IEnumerable<T> ts) => ts.ToImmutableList();
        public static IEnumerable<T> List<T>(params T[] items) => items.ToImmutableList();

    }
}