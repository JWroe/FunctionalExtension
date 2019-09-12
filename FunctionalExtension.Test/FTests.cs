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
        public void UsingDisposesObject()
        {
            new DisposeMe().Using(me => me).Invoke().Disposed.ShouldBeTrue();
        }

        [Fact]
        public void NotNegatesPredicate()
        {
             ((Func<bool>)(() => true)).Not().Invoke().ShouldBeFalse();
             ((Func<bool>)(() => false)).Not().Invoke().ShouldBeTrue();

        }
    }
}