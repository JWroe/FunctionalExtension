using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static FunctionalExtension.F;


namespace FunctionalExtension
{
    public sealed class Option<T> : IEquatable<Option<T>>
    {
        private readonly object _option;

        private Option([NotNull] object option) => _option = option;

        public R Match<R>(Func<Some<T>, R> ifSome, Func<None, R> ifNone) =>
#pragma warning disable 8509
            _option switch
#pragma warning restore 8509
            {
                Some<T> s => ifSome(s),
                None n => ifNone(n)
            };

        public static implicit operator Option<T>(None none) => new Option<T>(none);
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some);
        public static implicit operator Option<T>(T val) => val is null ? (Option<T>) None() : Some(val);
        public bool Equals(Option<T> other) => ReferenceEquals(this, other) || Equals(_option, other?._option);
        public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);
        public override int GetHashCode() => _option?.GetHashCode() ?? 0;
        public override string ToString() => $"Option:[ {_option} ]";
    }

    public sealed class Some<T> : IEquatable<Some<T>>, IEquatable<None>
    {
        public T Value { get; }

        internal Some([NotNull] T value) => Value = value;

        public static implicit operator T(Some<T> some) => some.Value;

        public bool Equals(Some<T> other) => (other != null) &&
                                             (ReferenceEquals(this, other) ||
                                              EqualityComparer<T>.Default.Equals(Value, other.Value));

        public bool Equals(None none) => false;
        public override bool Equals(object? obj) => obj is Some<T> other && Equals(other);
        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);
        public override string ToString() => $"Some[ {Value} ]";
    }

    public sealed class None : IEquatable<None>, IEquatable<object>
    {
        private None()
        {
        }

        internal static None Default => new None();
        public bool Equals(None other) => other != null;
        public override bool Equals(object? obj) => obj is None other && Equals(other);
        public override int GetHashCode() => 0;

        public override string ToString() => "None";
    }
    
    public static class OptionExt
    {
        public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) => optT.FlatMap(t => f(t).ReturnOption());
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
        public static Option<T> ReturnOption<T>(this T t) => t;
        public static IEnumerable<T> AsEnumerable<T>(this Option<T> optT) => optT.Match(some => List(some.Value), none => List<T>());
        public static IEnumerable<T> AsEnumerable<T>(this Some<T> some) => List(some.Value);
        public static IEnumerable<T> AsEnumerable<T>(this None _) => List<T>();
    }
}