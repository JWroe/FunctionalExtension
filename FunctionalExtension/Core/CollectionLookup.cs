using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace FunctionalExtension.Core
{
    public static partial class F
    {
        public static Option<TVal> Lookup<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key) where TKey : notnull => dict.TryGetValue(key, out var value) ? Some(value).AsOption() : None();
        public static Option<string> Lookup(this NameValueCollection @this, string key) => @this[key];

        public static Option<T> Lookup<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            var items = collection.Where(predicate);
            // ReSharper disable twice PossibleMultipleEnumeration - we are only looking at the first element, so it's better to do that twice than iterate over the whole collection once
            return items.Any() ? Some(items.First()).AsOption() : None();
        }
    }
}