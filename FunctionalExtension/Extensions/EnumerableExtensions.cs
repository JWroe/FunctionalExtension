using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FunctionalExtension.Types;
using static FunctionalExtension.Core.F;
using Unit = System.ValueTuple;

namespace FunctionalExtension.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) => ts.Select(f);
        public static ISet<R> Map<T, R>(this ISet<T> ts, Func<T, R> f) => ts.Select(f).ToImmutableHashSet();

        public static IDictionary<RKey, R> Map<Key, T, RKey, R>(this IDictionary<Key, T> ts,
                                                                Func<KeyValuePair<Key, T>, RKey> keySelector,
                                                                Func<KeyValuePair<Key, T>, R> valSelector) where Key : notnull where RKey : notnull
            => ts.ToImmutableDictionary(keySelector, valSelector);

        public static IImmutableDictionary<Key, T> ToImmutableDictionary<T, Key>(this IEnumerable<T> ts,
                                                                                 Func<T, Key> keySelector) where Key : notnull
            => ts.ToImmutableDictionary(keySelector, t => t);

        public static IEnumerable<R> FlatMap<T, R>(this IEnumerable<T> ts, Func<T, IEnumerable<R>> f) => ts.SelectMany(f);

        public static IEnumerable<R> FlatMap<T, R>(this IEnumerable<T> list, Func<T, Option<R>> func) => list.FlatMap(t => func(t).AsEnumerable());
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> f) => ts.Where(f);

        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action) => ts.Map(action.ToFunc()).Execute();
        public static IEnumerable<T> Execute<T>(this IEnumerable<T> ts) => ts.ToImmutableList();
        public static IEnumerable<T> ReturnCollection<T>(this T t) => List(t);
    }
}