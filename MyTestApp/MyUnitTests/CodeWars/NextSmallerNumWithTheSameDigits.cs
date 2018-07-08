using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyUnitTests.CodeWars
{
    // http://www.codewars.com/kata/next-smaller-number-with-the-same-digits/train/csharp

    static class Ex
    {
        public static long ToLong(this IEnumerable<char> arr)
        {
            return long.Parse(new string(arr.ToArray()));
        }

    }

    class NextSmallerNumWithTheSameDigits
    {
        private static long MyPow(long x, long y)
        {
            if (y == 1) return x;
            return x * MyPow(x, y - 1);
        }

        public static long NextSmaller(long n)
        {
            if (n < 21) return -1;

            var nums = n.ToString().Select(x => x).ToArray();
            var last = nums.Length - 1;

            if (n % MyPow(10, last) == 0) return -1;

            var dic = nums.Select((x, i) => new {Pos = i, Value = x}).ToList();

            try
            {
                var small = dic.Last(x => dic.Any(sx => sx.Value > x.Value && sx.Pos < x.Pos) && x.Value != '0');
                var big = dic.Last(x => x.Value > small.Value && x.Pos < small.Pos);
                var maxSmall = dic.FirstOrDefault(x => x.Value > small.Value && x.Pos > big.Pos && x.Pos < small.Pos) ??
                               small;

                var res = nums.Take(big.Pos).ToList();
                res.Add(maxSmall.Value);
                res.AddRange(
                    dic.Skip(big.Pos)
                        .Where(x => x.Pos != maxSmall.Pos)
                        .Select(x => x.Value)
                        .OrderByDescending(x => x)
                );

                return res.ToLong();
            }
            catch
            {
            }

            try
            {
                var zeroSmall = dic.First(x => x.Value == '0');
                var zeroBig = dic.Last(x => x.Value == '0');
                // var zeroCnt = dic.Count(x => x.Value == '0');
                var nonZero = dic.First(x => x.Value != '0' && x.Pos > zeroSmall.Pos);
                var nonZeroBetween = nonZero.Pos < zeroBig.Pos;

                if (zeroSmall.Pos == last)
                {
                    // swap last 2 digits
                    var swap = nums[last];
                    nums[last] = nums[last - 1];
                    nums[last - 1] = swap;
                    return nums.ToLong();
                }

                List<char> res;

                if (zeroSmall.Pos == 1) // second
                {
                    if (dic.Any(x => x.Pos > 1 && x.Value < nums[0]))
                    {
                        var first = dic.Skip(2).Where(x => x.Value < nums[0]).Max();
                        res = new List<char> {first.Value};
                        res.AddRange(dic.Where(x => x.Pos != first.Pos).Select(x => x.Value).OrderByDescending(x => x));
                        return res.ToLong();
                    }

                    return -1;
                }

                res = nums.Take(zeroSmall.Pos - 1).ToList();
                res.Add('0');
                var rest = new List<char> {nums[zeroSmall.Pos - 1]};
                rest.AddRange(nums.Skip(zeroSmall.Pos+1));
                res.AddRange(rest.OrderByDescending(x => x));
                return rest.ToLong();
            }
            catch
            {
            }

            return -1;
        }
    }

    [TestClass]
    public class Tests
    {
        [TestMethod]
        [DataRow(29009, 20990)]
        [DataRow(1027, -1)]
        [DataRow(2071, 2017)]
        [DataRow(809, -1)]
        [DataRow(908, 890)]
        [DataRow(1234567908, 1234567890)]
        [DataRow(315, 153)]
        [DataRow(907, 790)]
        [DataRow(100, -1)]
        [DataRow(531, 513)]
        [DataRow(21, 12)]
        [DataRow(441, 414)]
        [DataRow(123456798, 123456789)]
        public void FixedTests(long n, long exRes) 
            => Assert.AreEqual(exRes, NextSmallerNumWithTheSameDigits.NextSmaller(n));
    }
}
