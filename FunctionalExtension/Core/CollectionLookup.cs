using System.Collections.Generic;
using System.Collections.Specialized;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Option<TVal> Lookup<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key) where TKey : notnull => dict.TryGetValue(key, out var value) ? Some(value).AsOption() : None();
        public static Option<string> Lookup(this NameValueCollection @this, string key) => @this[key];
    }
}