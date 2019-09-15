using System.Collections.Generic;
using Shouldly;
using Xunit;
using static FunctionalExtension.Core.F;

namespace FunctionalExtension.Test
{
    public class OptionTypesTests
    {
        private const string NoneMatched = "This was a None option";
        private const string SomeMatched = "This was a Some option";

        [Fact]
        public void SomeGetsValuePassed() => Some("Frederick").Value.ShouldBe("Frederick");

        [Theory]
        [MemberData(nameof(PatternMatchOptions))]
        public void OptionTypePatternMatchingIsCorrect(Option<string> input, string expected) =>
            input.Match(some => SomeMatched, none => NoneMatched).ShouldBe(expected);

        public static IEnumerable<object[]> PatternMatchOptions =>
            new List<object[]>
            {
                new object[] { None(), NoneMatched },
                new object[] { Some(SomeMatched), SomeMatched },
            };

        [Theory]
        [MemberData(nameof(MapOptions))]
        public void OptionMapsCorrectly(Option<string> input, Option<string> expected) =>
            input.Map(str => str.ToUpper()).ShouldBe(expected);

        public static IEnumerable<object[]> MapOptions =>
            new List<object[]>
            {
                new object[] { None(), None() },
                new object[] { Some(SomeMatched), Some(SomeMatched.ToUpper()) },
            };

        [Fact]
        public void BindIsCorrect() => Some(6).FlatMap(i => i * 2).ShouldBe(Some(12).AsOption());

        [Fact]
        public void SomeOptionAsEnumerableMapsCorrectly() => Some(1).AsOption().AsEnumerable().ShouldBe(List(1));

        [Fact]
        public void NoneAsEnumerableMapsCorrectly() => None().AsOption<int>().AsEnumerable().ShouldBe(List<int>());
    }
}