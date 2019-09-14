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
        public void NameValueCollectionLookup(string input, string expected) =>
            new NameValueCollection { { "name", "value" } }.Lookup(input)
                                                           .Match(some => some, none => "not found")
                                                           .ShouldBe(expected);

        [Theory]
        [InlineData("key", "value")]
        [InlineData("not key", "not found")]
        public void DictionaryLookup(string input, string expected) =>
            new Dictionary<string, string> { { "key", "value" } }.Lookup(input)
                                                                 .Match(some => some, none => "not found")
                                                                 .ShouldBe(expected);

        [Theory]
        [InlineData("one", "one")]
        [InlineData("can't find me'", "not found")]
        public void FirstOrNoneTest(string wordToFind, string expected) =>
            new List<string> { "one", "two", "three" }.Lookup(word => word == wordToFind)
                                                      .Match(some => some, none => "not found")
                                                      .ShouldBe(expected);

        [Theory]
        [InlineData(1, 1)]
        [InlineData(15, -1)]
        public void FirstOrNoneTestStructs(int wordToFind, int expected) =>
            new List<int> { 1 }.Lookup(word => word == wordToFind)
                               .Match(some => some, none => -1)
                               .ShouldBe(expected);
    }
}