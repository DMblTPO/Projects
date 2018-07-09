using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyUnitTests.CodeWars
{
    // http://www.codewars.com/kata/next-smaller-number-with-the-same-digits/train/csharp

    class Pair
    {
        public int Ind { get; set; }
        public char Chr { get; set; }
    }

    static class Ex
    {
        public static long ToLong(this IEnumerable<char> arr)
        {
            return long.Parse(new string(arr.ToArray()));
        }

        public static char[] ToCharArray(this long n)
        {
            return n.ToString().Select(x => x).ToArray();
        }

        public static IList<Pair> ToDic(this char[] arr)
        {
            return arr.Select((x, i) => new Pair{Ind = i, Chr = x}).ToList();
        }
    }

    class NextNumWithTheSameDigits
    {
        private static long MyPow(long x, long y)
        {
            if (y == 1) return x;
            return x * MyPow(x, y - 1);
        }

        public static long NextBigger(long n)
        {
            if (n < 12) return -1;

            var nums = n.ToCharArray();
            var last = nums.Length - 1;

            if (n % MyPow(10, last) == 0) return -1;

            var dic = nums.ToDic();

            try
            {
                var zero = dic.Last(x => x.Chr == '0');
                var zzero = dic.LastOrDefault(x => x.Chr == '0' && x.Ind < zero.Ind) ?? zero;
                if (zzero.Ind != last)
                {
                    var replace = dic.Where(x => x.Ind > zzero.Ind && x.Chr != '0').Min();
                    var res = new List<char>(nums.Take(zzero.Ind)) {replace.Chr, '0'};
                    res.AddRange(
                        dic.Where(x => x.Ind > zzero.Ind && x.Ind != replace.Ind)
                           .Select(x => x.Chr)
                           .OrderBy(x => x)
                        );
                    return res.ToLong();
                }
            }
            catch
            {
            }

            try
            {
                var first = dic.Last(x => dic.Take(x.Ind).Any(sx => sx.Chr < x.Chr));
                var replace = dic.Last(x => x.Chr < first.Chr); // zero's handled upper
                var res = new List<char>(nums.Take(replace.Ind)) {first.Chr};
                res.AddRange(
                    dic.Skip(replace.Ind + 1)
                        .Where(x => x.Ind != first.Ind)
                         .Select(x => x.Chr)
                          .OrderBy(x => x));
                return res.ToLong();
            }
            catch
            {
            }

            return -1;
        }

        public static long NextSmaller(long n)
        {
            if (n < 21) return -1;

            var nums = n.ToCharArray();
            var last = nums.Length - 1;

            if (n % MyPow(10, last) == 0) return -1;

            var dic = nums.ToDic();

            if (nums[last] != '0')
            {
                try
                {
                    var small = dic.Last(x => dic.Any(sx => sx.Chr > x.Chr && sx.Ind < x.Ind) && x.Chr != '0');
                    var big = dic.Last(x => x.Chr > small.Chr && x.Ind < small.Ind);
                    var maxSmall =
                        dic.FirstOrDefault(x => x.Chr > small.Chr && x.Ind > big.Ind && x.Ind < small.Ind) ??
                        small;

                    var res = nums.Take(big.Ind).ToList();
                    res.Add(maxSmall.Chr);
                    res.AddRange(
                        dic.Skip(big.Ind)
                            .Where(x => x.Ind != maxSmall.Ind)
                            .Select(x => x.Chr)
                            .OrderByDescending(x => x)
                    );

                    return res.ToLong();
                }
                catch
                {
                }
            }

            try
            {
                var zeroFirst = dic.First(x => x.Chr == '0');
                var zeroLast = dic.Last(x => x.Chr == '0');
                var nonZero = dic.LastOrDefault(x => x.Chr != '0' && x.Ind > zeroFirst.Ind && x.Ind < zeroLast.Ind);
                if (nonZero != null)
                {
                    zeroFirst = dic.First(x => x.Chr == '0' && x.Ind > nonZero.Ind);
                }

                List<char> res;

                if (zeroFirst.Ind == 1) // second
                {
                    if (dic.Any(x => x.Ind > 1 && x.Chr < nums[0]))
                    {
                        var first = dic.Skip(2).Where(x => x.Chr < nums[0]).Max();
                        res = new List<char> {first.Chr};
                        res.AddRange(dic.Where(x => x.Ind != first.Ind).Select(x => x.Chr).OrderByDescending(x => x));
                        return res.ToLong();
                    }

                    return -1;
                }

                res = nums.Take(zeroFirst.Ind - 1).ToList();
                res.Add('0');
                var rest = new List<char> {nums[zeroFirst.Ind - 1]};
                rest.AddRange(nums.Skip(zeroFirst.Ind+1));
                res.AddRange(rest.OrderByDescending(x => x));
                return res.ToLong();
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
        [DataRow(125130, 125103)]
        [DataRow(315, 153)]
        [DataRow(908, 890)]
        [DataRow(-1, 980)]
        [DataRow(29009, 20990)]
        [DataRow(2071, 2017)]
        [DataRow(1702, 1072)]
        [DataRow(-1, 100)]
        [DataRow(125301, 125130)]
        [DataRow(12513531, 12513513)]
        [DataRow(12531, 12513)]
        [DataRow(21, 12)]
        [DataRow(441, 414)]
        [DataRow(531, 513)]
        [DataRow(1234567908, 1234567890)]
        [DataRow(907, 790)]
        [DataRow(123456798, 123456789)]
        [DataRow(10829494039415810, 10829494039415801)]
        public void TestNextBigger(long exRes, long n) 
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextBigger(n));

        [TestMethod]
        [DataRow(10829494039415810, 10829494039415801)]
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
        public void TestNextSmaller(long n, long exRes) 
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextSmaller(n));

    }
}
