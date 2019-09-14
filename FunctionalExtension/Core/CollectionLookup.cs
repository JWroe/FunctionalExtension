using System.Collections.Specialized;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Option<string> Lookup(this NameValueCollection @this, string key) => @this[key];

    }
}