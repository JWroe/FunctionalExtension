namespace FunctionalExtension.Core
{
    public partial class F
    {
        public static Option<int> ParseInt(this string str) => int.TryParse(str, out var num) ? Some(num).AsOption() : None();
        public static Option<double> ParseDouble(this string str) => double.TryParse(str, out var dbl) ? Some(dbl).AsOption() : None();

    }
}