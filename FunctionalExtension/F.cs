using System;

namespace FunctionalExtension
{
    public static class F
    {
        public static R Using<T, R>(T disposable, Func<T, R> f) where T : IDisposable
        {
            using (disposable) return f(disposable);
        }

        public static Func<bool> Not(Func<bool> predicate) => () => !predicate();
    }
}