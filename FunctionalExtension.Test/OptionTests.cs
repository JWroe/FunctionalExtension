﻿using System.Collections.Generic;
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
        public void BindIsCorrect()
        {
            var r = "6".ParseInt().Bind(Age.Of);
        }

        public sealed class Age
        {
            private readonly int value;
            public static Option<Age> Of(int age) => IsValid(age) ? Some(new Age(age)).AsOption() : None();

            private static bool IsValid(int age) => age >= 0 && age <= 120;

            private Age(int value) => this.value = value;

        }
    }
}