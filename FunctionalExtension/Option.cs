using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FunctionalExtension.Exceptions;
using static FunctionalExtension.Core.F;

namespace FunctionalExtension
{
    public sealed class Option<T> : IEquatable<Option<T>>
    {
        private readonly object option;

        private Option([NotNull] object option) => this.option = option;
        
        public R Match<R>(Func<Some<T>, R> ifSome, Func<None, R> ifNone) =>
            option switch
            {
                Some<T> s => ifSome(s),
                None n => ifNone(n),
                _ => throw new ThisCantHappenException()
            };

        public static implicit operator Option<T>(None none) => new Option<T>(none);
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some);
        public static implicit operator Option<T>(T val) => val is null ? (Option<T>)None() : Some(val);

        public bool Equals(Option<T> other) => ReferenceEquals(this, other) || Equals(option, other?.option);
        public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);
        public override int GetHashCode() => option?.GetHashCode() ?? 0;
        public override string ToString() => $"Option:[ {option} ]";
    }

    public sealed class Some<T> : IEquatable<Some<T>>, IEquatable<None>
    {
        public T Value { get; }

        internal Some([NotNull] T value) => Value = value;

        public static implicit operator T(Some<T> some) => some.Value;

        public bool Equals(Some<T> other) => other != null && (ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(Value, other.Value));
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
}