using System.Collections.Specialized;
using Unit = System.ValueTuple;

namespace FunctionalExtension
{
    public static partial class F
    {
        public static Option<string> Lookup(this NameValueCollection @this, string key) => @this[key];

    }
}