using System;
using Shouldly;
using static FunctionalExtension.F;
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
            var toDispose = Using(new DisposeMe(), me => me);
            
            toDispose.Disposed.ShouldBeTrue();
        }
    }
}