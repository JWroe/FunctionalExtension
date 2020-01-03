using Shouldly;
using Xunit;
using static System.Math;

namespace FunctionalExtension.Test
{
    public class EitherTests
    {
        private static Either<string, double> Calc(double x, double y)
        {
            if (Abs(y) < 0.01) return "y cannot be 0";

            if (Abs(x) > 0.01 && Sign(x) != Sign(y))
                return "x / y cannot be negative";

            return Sqrt(x / y);
        }

        [Theory]
        [InlineData(-5, 2, "x / y cannot be negative")]
        [InlineData(5, 0, "y cannot be 0")]
        public void LeftTest(int x, int y, string expected) =>
            Calc(x, y).Match(left => left, right => "").ShouldBe(expected);

        [Theory]
        [InlineData(true, true, false, "Failed interview")]
        [InlineData(true, false, true, "Failed tech test")]
        [InlineData(false, true, true, "isn't eligible")]
        public void RecruitmentProcess_Fails(bool isEligible, bool passedTechTest, bool passedInterview,
            string expectedReason)
        {
            Either<Reason, Candidate> IsEligible(Candidate candidate) => isEligible
                ? candidate.AsEitherRight<Reason, Candidate>()
                : new Reason("isn't eligible");

            Either<Reason, Candidate> TechTest(Candidate candidate) => passedTechTest
                ? candidate.AsEitherRight<Reason, Candidate>()
                : new Reason("Failed tech test");

            Either<Reason, Candidate> Interview(Candidate candidate) => passedInterview
                ? candidate.AsEitherRight<Reason, Candidate>()
                : new Reason("Failed interview");

            Either<Reason, Candidate> Recruit(Candidate c)
                => c.AsEitherRight<Reason, Candidate>()
                    .Bind(IsEligible)
                    .Bind(TechTest)
                    .Bind(Interview);

            Recruit(new Candidate("Dave"))
                .Match(reason => reason.Text, candidate => "Test failed")
                .ShouldBe(expectedReason);
        }

        [Fact]
        public void RightTest() => Calc(18, 2).Match(left => 0, right => right).ShouldBe(3d);
    }

    internal class Reason
    {
        public Reason(string reason) => Text = reason;

        public string Text { get; }
    }

    internal class Candidate
    {
        public Candidate(string name) => Name = name;
        public string Name { get; }
    }
}