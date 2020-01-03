// CC https://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way

using System;
using System.Linq;

namespace FunctionalExtension
{
    public static partial class F
    {
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min) throw new ArgumentOutOfRangeException(nameof(max), "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            var uRange = (ulong)(max - min);

            //Prevent a modulo bias; see https://stackoverflow.com/a/10984975/238419 for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                var buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        public static long NextLong(this Random random, long max) => random.NextLong(0, max);

        public static long NextLong(this Random random) => random.NextLong(long.MaxValue);
        public static DateTime NextDateTime(this Random random) => new DateTime(random.NextLong(DateTime.MaxValue.Ticks));

        // ReSharper disable twice StringLiteralTypo
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string NextString(this Random random, int length = 8) => new string(Range(length).Map(_ => Chars[random.Next(Chars.Length)]).ToArray());
    }
}