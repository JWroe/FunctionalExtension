using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace FunctionalExtension
{
    public static partial class F
    {
        public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
        public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);

        public static Either<L, R> AsEitherLeft<L, R>(this L l) => new Either<L, R>(l);
        public static Either<L, R> AsEitherRight<L, R>(this R r) => new Either<L, R>(r);
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

        public Unit Match(Action<L> left, Action<R> right)
            => Match(left.ToFunc(), right.ToFunc());

        public Tr Match<Tr>(Func<L, Tr> left, Func<R, Tr> right)
            => IsLeft ? left(Left) : right(Right);

        public Either<Lr, Rr> Map<Lr, Rr>(Func<L, Lr> left, Func<R, Rr> right)
            => IsRight ? right(Right).AsEitherRight<Lr, Rr>() : left(Left);

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
        public static Either<L, Rr> Map<L, R, Rr>(this Either<L, R> @this, Func<R, Rr> f)
            => @this.Match<Either<L, Rr>>(left => left, r => f(r));

        public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> act)
            => Map(@this, act.ToFunc());

        public static Either<L, Rr> Bind<L, R, Rr>(this Either<L, R> @this, Func<R, Either<L, Rr>> f)
            => @this.Match(left => left, f);
    }
}