using System;
using Shouldly;
using Xunit;

namespace FunctionalExtension.Test
{
    public class FTests
    {
        private class DisposeMe : IDisposable
        {
            public bool Disposed { get; private set; }
            public void Dispose() => Disposed = true;
        }

        [Fact]
        public void UsingDisposesObject() => new DisposeMe().Using(me => me).Invoke().Disposed.ShouldBeTrue();

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void NotNegatesPredicate(bool input, bool expected)
            => ((Func<bool>)(() => input)).Not().Invoke().ShouldBe(expected);

        [Theory]
        [InlineData("10", 10)]
        [InlineData("test", -1)]
        [InlineData("2147483648", -1)]
        public void ParseStringToInt(string input, int expected) =>
            input.Parse()
                 .Match(some => some.Value, none => -1)
                 .ShouldBe(expected);
    }
}