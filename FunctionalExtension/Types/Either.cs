using System;
using System.Collections.Generic;

namespace FunctionalExtension
{
    public static partial class F
    {
        public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
        public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
    }

    public sealed class Either<L, R>
    {
        public Either(L left)
        {
            IsRight = false;
            Left = left;
            Right = default!;
        }

        public Either(R right)
        {
            IsRight = true;
            Right = right;
            Left = default!;
        }

        internal L Left { get; }
        internal R Right { get; }

        private bool IsRight { get; }
        private bool IsLeft => !IsRight;

        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);

        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);
        public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

        public Tr Match<Tr>(Func<L, Tr> left, Func<R, Tr> right)
            => IsLeft ? left(Left) : right(Right);

         public ValueTuple Match(Action<L> left, Action<R> right)
             => Match(left.ToFunc(), right.ToFunc());
        
         public IEnumerator<R> AsEnumerable()
         {
             if (IsRight) yield return Right;
         }

        public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
    }

    public static class Either
    {
        public sealed class Left<L>
        {
            internal Left(L value) => Value = value;
            internal L Value { get; }

            public override string ToString() => $"Left({Value})";
        }

        public sealed class Right<R>
        {
            internal Right(R value) => Value = value;
            internal R Value { get; }

            public override string ToString() => $"Right({Value})";

            public Right<Rr> Map<Rr>(Func<R, Rr> f) => F.Right(f(Value));
            public Either<L, Rr> Bind<L, Rr>(Func<R, Either<L, Rr>> f) => f(Value);
        }
    }

    public static class EitherExt
    {
        public static Either<L, Rr> Map<L, R, Rr>
            (this Either<L, R> @this, Func<R, Rr> f)
            => @this.Match<Either<L, Rr>>(
                F.Left,
                r => F.Right(f(r)));

        public static Either<Ll, Rr> Map<L, Ll, R, Rr>
            (this Either<L, R> @this, Func<L, Ll> left, Func<R, Rr> right)
            => @this.Match<Either<Ll, Rr>>(
                l => F.Left(left(l)),
                r => F.Right(right(r)));

        public static Either<L, ValueTuple> ForEach<L, R>
            (this Either<L, R> @this, Action<R> act)
            => Map(@this, act.ToFunc());

        public static Either<L, Rr> Bind<L, R, Rr>
            (this Either<L, R> @this, Func<R, Either<L, Rr>> f)
            => @this.Match(
                F.Left,
                f);

        public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
            , Func<T, R> map) => @this.Map(map);


        public static Either<L, Rr> SelectMany<L, T, R, Rr>(this Either<L, T> @this
            , Func<T, Either<L, R>> bind, Func<T, R, Rr> project)
            => @this.Match(
                F.Left,
                t =>
                    bind(@this.Right).Match<Either<L, Rr>>(
                        F.Left,
                        r => project(t, r)));
    }
}