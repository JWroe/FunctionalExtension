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
        [InlineData(15, "value")]
        [InlineData(0, "not found")]
        public void DictionaryLookup(int input, string expected)
        {
            var collection = new Dictionary<int, string> { { 15, "value" } };
            collection.Lookup(input)
                      .Match(some => some, none => "not found")
                      .ShouldBe(expected);
        }
    }
}