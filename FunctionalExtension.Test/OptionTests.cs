using Shouldly;
using Xunit;
using static FunctionalExtension.F;

namespace FunctionalExtension.Test
{
    public class OptionTypesTests
    {
        private const string NoneMatched = "This was a None option";
        private const string SomeMatched = "This was a Some option";

        [Fact]
        public void SomeGetsValuePassed() => Some("Frederick").Value.ShouldBe("Frederick");

        [Fact]
        public void SomePatternMatchingIsCorrect() =>
            Some(SomeMatched).Match(some => SomeMatched.ToUpper(), none => NoneMatched).ShouldBe(SomeMatched.ToUpper());

        [Fact]
        public void OptionTypePatternMatchingIsCorrect() =>
            None().AsOption<string>().Match(some => SomeMatched.ToUpper(), none => NoneMatched).ShouldBe(NoneMatched);

        [Fact]
        public void SomeOptionMapsCorrectly()
            => Some(SomeMatched).Map(str => str.ToUpper()).ShouldBe(SomeMatched.ToUpper());

        [Fact]
        public void NoneOptionMapsCorrectly()
            => None().AsOption<string>().Map(s => s.ToUpper()).ShouldBe(None());

        [Fact]
        public void BindIsCorrect()
            => Some(6).FlatMap(i => Some(i * 2)).ShouldBe(Some(12).AsOption());

        [Fact]
        public void SomeOptionAsEnumerableMapsCorrectly()
            => Some(1).AsEnumerable().ShouldBe(List(1));

        [Fact]
        public void NoneAsEnumerableMapsCorrectly()
            => None().AsEnumerable<int>().ShouldBe(List<int>());
    }
}