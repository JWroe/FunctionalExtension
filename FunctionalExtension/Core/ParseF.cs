using System;
using FunctionalExtension.Extensions;
using FunctionalExtension.Types;

namespace FunctionalExtension.Core
{
    public partial class F
    {
        public static Option<int> ParseInt(this string str) => int.TryParse(str, out var num) ? Some(num).AsOption() : None();
        public static Option<double> ParseDouble(this string str) => double.TryParse(str, out var dbl) ? Some(dbl).AsOption() : None();
        public static Option<T> ParseEnum<T>(this string str) where T : struct, Enum => Enum.TryParse<T>(str, out var @enum) ? Some(@enum).AsOption() : None();
    }
}