using System.Diagnostics.CodeAnalysis;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static None None() => FunctionalExtension.None.Default;
        public static Option<T> AsOption<T>([NotNull] this None none) => none;
        public static Some<T> Some<T>([NotNull] T value) => new Some<T>(value);
        public static Option<T> AsOption<T>([NotNull] this Some<T> some) => some;
    }
}