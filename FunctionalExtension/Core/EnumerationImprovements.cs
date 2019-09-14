using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) => ts.Select(f);
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) => source.Select(selector);

        public static IEnumerable<TResult> FlatMap<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
            => source.SelectMany(selector);

        public static IEnumerable<TResult> FlatMap<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
            => source.SelectMany(selector);

        public static IEnumerable<TResult> FlatMap<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            => source.SelectMany(collectionSelector, resultSelector);

        public static IEnumerable<TResult> FlatMap<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            => source.SelectMany(collectionSelector, resultSelector);

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> ts, Func<T, bool> f) => ts.Where(f);
        
        public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> ts, Action<T> action) => ts.Map(action.ToFunc()).Execute();
        public static IEnumerable<T> Execute<T>(this IEnumerable<T> ts) => ts.ToImmutableList();
    }
}