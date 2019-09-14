using System;
using System.Collections.Generic;
using FunctionalExtension.Core;
using Shouldly;
using Xunit;
using static FunctionalExtension.Core.F;

namespace FunctionalExtension.Test
{
    public class ParseTests
    {
        [Theory]
        [InlineData("10", 10)]
        [InlineData("test", -1)]
        [InlineData("2147483648", -1)]
        public void ParseStringToInt(string input, int expected) =>
            input.ParseInt()
                 .Match(some => some.Value, none => -1)
                 .ShouldBe(expected);

        [Theory]
        [InlineData("10.5", 10.5)]
        [InlineData("test", -1)]
        [InlineData("1.79769313486231E+308", 1.79769313486231E+308)]
        [InlineData("4.94065645841247E-324", 4.94065645841247E-324)]
        [InlineData("-1.79769313486231E+308", -1.79769313486231E+308)]
        public void ParseStringToDouble(string input, double expected) =>
            input.ParseDouble()
                 .Match(some => some.Value, none => -1)
                 .ShouldBe(expected);

        [Theory]
        [MemberData(nameof(TestEnumData))]
        public void ParseStringToEnum(string input, object expected) =>
            input.ParseEnum<DayOfWeek>().ShouldBe(expected);

        public static IEnumerable<object[]> TestEnumData =>
            new List<object[]>
            {
                new object[] { "Friday", Some(DayOfWeek.Friday).AsOption() },
                new object[] { "Freeday", None().AsOption<DayOfWeek>()},
            };
    }
}