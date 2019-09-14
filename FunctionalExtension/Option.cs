using System;

namespace FunctionalExtension
{
    public sealed class Option<T>
    {
        private readonly object option;

        private Option(object option) => this.option = option;

        public R Match<R>(Func<Some<T>, R> ifSome, Func<None, R> ifNone) =>
            option switch
            {
                Some<T> s => ifSome(s),
                None n => ifNone(n),
                _ => throw new ThisCantHappenException()
            };

        public static implicit operator Option<T>(None none) => new Option<T>(none);
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some);
    }

    public sealed class Some<T>
    {
        public T Value { get; }

        internal Some(T value) => Value = value;

        public static implicit operator T(Some<T> some) => some.Value;
    }

    public sealed class None
    {
        private None()
        {
        }

        internal static None Default => new None();
    }
}