using System;
using System.Linq;
using FunctionalExtension.Core;
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

        [Fact]
        public void ForEachWorksCorrectly()
        {
            var count = 0;
            Enumerable.Range(1, 5).ForEach(i => count += 1);
            count.ShouldBe(5);
        }
    }
}