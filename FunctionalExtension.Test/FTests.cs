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
    }
}