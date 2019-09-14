using System.Diagnostics.CodeAnalysis;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static None None() => FunctionalExtension.None.Default;
        public static Some<T> Some<T>([NotNull] T value) => new Some<T>(value);
        
        public static Option<T> AsOption<T>(this Some<T> some) => some;
        public static Option<T> AsOption<T>(this None none) => none;


    }
}