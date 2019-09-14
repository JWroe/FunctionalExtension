using System;
using System.Collections.Specialized;
using Shouldly;
using Xunit;

namespace FunctionalExtension.Test
{
    public class LookupsTests
    {
        [Theory]
        [InlineData("name", "value")]
        [InlineData("something else", "not found")]
        public void ParseStringToInt(string input, string expected)
        {
            var collection = new NameValueCollection { { "name", "value" } };
            collection.Lookup(input)
                      .Match(some => some, none => "not found")
                      .ShouldBe(expected);
        }
    }
}