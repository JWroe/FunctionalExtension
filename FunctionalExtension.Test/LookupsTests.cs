using System.Collections.Generic;
using System.Collections.Specialized;
using FunctionalExtension.Core;
using Shouldly;
using Xunit;

namespace FunctionalExtension.Test
{
    public class LookupsTests
    {
        [Theory]
        [InlineData("name", "value")]
        [InlineData("something else", "not found")]
        public void NameValueCollectionLookup(string input, string expected)
        {
            var collection = new NameValueCollection { { "name", "value" } };
            collection.Lookup(input)
                      .Match(some => some, none => "not found")
                      .ShouldBe(expected);
        }
        
        [Theory]
        [InlineData("key", "value")]
        [InlineData("not key", "not found")]
        public void DictionaryLookup(string input, string expected)
        {
            var collection = new Dictionary<string, string> { { "key", "value" } };
            collection.Lookup(input)
                      .Match(some => some, none => "not found")
                      .ShouldBe(expected);
        }
    }
}