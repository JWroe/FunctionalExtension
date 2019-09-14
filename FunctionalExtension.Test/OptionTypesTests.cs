using System;
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
        public void SomeGetsValuePassed() => Some("Frederick").Value
                                                             .ShouldBe("Frederick");

        [Theory]
        [MemberData(nameof(Options))]
        public void OptionMapsCorrectly(Func<Option<string>> getOption, string expected) =>
            getOption().Match(some => SomeMatched, none => NoneMatched)
                       .ShouldBe(expected);

        public static IEnumerable<object[]> Options =>
            new List<object[]>
            {
                new object[] { (Func<Option<string>>)(() => None()), NoneMatched },
                new object[] { (Func<Option<string>>)(() => Some(SomeMatched)), SomeMatched },
            };
    }
}