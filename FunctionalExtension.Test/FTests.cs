using System;
using Shouldly;
using Xunit;
using static FunctionalExtension.F;

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
            var toDispose = Using(new DisposeMe(), me => me);
            
            toDispose.Disposed.ShouldBeTrue();
        }

        [Fact]
        public void NotNegatesPredicate()
        {
            bool Predicate(bool input) => input;
            Not(() => Predicate(true)).Invoke().ShouldBe(false);
            Not(() => Predicate(false)).Invoke().ShouldBe(true);
        }
    }
}