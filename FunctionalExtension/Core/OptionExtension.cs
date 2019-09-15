using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) => optT.Match(some => f(some.Value), _ => None().AsOption<R>());
        public static Option<R> Map<T, R>(this Some<T> some, Func<T, R> f) => some.AsOption().Map(f);
        public static Option<R> FlatMap<T, R>(this Option<T> optT, Func<T, Option<R>> f) => optT.Match(some => f(some.Value), none => none);
        public static Option<R> FlatMap<T, R>(this Some<T> optT, Func<T, Option<R>> f) => ((Option<T>)optT).FlatMap(f);
        public static Option<R> FlatMap<T, R>(this Some<T> optT, Func<T, Some<R>> f) => optT.AsOption().FlatMap(t => f(t).AsOption());
        public static Option<R> FlatMap<T, R>(this Some<T> optT, Func<T, None> f) => optT.AsOption().FlatMap(t => f(t).AsOption<R>());

        public static IEnumerable<R> FlatMap<T, R>(this Option<T> opt, Func<T, IEnumerable<R>> func) => opt.AsEnumerable().FlatMap(func);

        public static Option<T> Filter<T>(this Option<T> ts, Func<T, bool> f) => ts.Match(some => f(some) ? Some(some.Value).AsOption() : None(), none => none);

        public static R Match<T, R>(this Some<T> some, Func<Some<T>, R> fSome, Func<None, R> fNone) => some.AsOption().Match(fSome, fNone);

        public static Option<T> AsOption<T>(this Some<T> some) => some;
        public static Option<T> AsOption<T>(this None none) => none;
        public static IEnumerable<T> AsEnumerable<T>(this Option<T> optT) => optT.Match(some => List(some.Value), none => List<T>());
        public static IEnumerable<T> AsEnumerable<T>(this Some<T> some) => List(some.Value);
        public static IEnumerable<T> AsEnumerable<T>(this None _) => List<T>();
    }
}