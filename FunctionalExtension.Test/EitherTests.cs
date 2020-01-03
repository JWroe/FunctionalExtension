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

        [Fact]
        public void RightTest() => Calc(18, 2).Match(left => 0, right => right).ShouldBe(3d);
        
        [Theory]
        [InlineData(-5, 2, "x / y cannot be negative")]
        [InlineData(5, 0, "y cannot be 0")]
        public void LeftTest(int x, int y, string expected) => Calc(x, y).Match(left => left, right => "").ShouldBe(expected);
    }
}