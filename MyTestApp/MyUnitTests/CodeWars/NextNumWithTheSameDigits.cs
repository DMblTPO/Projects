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
                var lhv = dic.Last(x => x.Ind != last && dic.Any(sx => sx.Ind > x.Ind && sx.Chr > x.Chr));
                var rhv = dic.Last(x => x.Chr > lhv.Chr && x.Ind > lhv.Ind && x.Chr != '0');
                var res = new List<char>(nums.Take(lhv.Ind)) {rhv.Chr};
                res.AddRange(
                    dic.Where(x => x.Ind >= lhv.Ind && x.Ind != rhv.Ind && x.Ind != last + 1)
                        .Select(x => x.Chr)
                        .OrderBy(x => x));
                return res.ToLong();
            }
            catch
            {
            }

            try
            {
                var zero = dic.Last(x => x.Chr == '0' && 
                                         dic.SingleOrDefault(
                                             sx => sx.Ind == x.Ind + 1 &&
                                                   sx.Chr > '0'
                                             ) != null);
                if (dic.Skip(zero.Ind).All(x => x.Chr <= dic[zero.Ind + 1].Chr))
                {
                    var ch = dic.Where(x => x.Ind > zero.Ind && x.Chr != '0').Min(x => x.Chr);
                    var replace = dic.First(x => x.Ind > zero.Ind && x.Chr == ch);
                    var res = new List<char>(nums.Take(zero.Ind)) {replace.Chr, '0'};
                    res.AddRange(
                        dic.Where(x => x.Ind > zero.Ind && x.Ind != replace.Ind)
                            .Select(x => x.Chr)
                            .OrderBy(x => x)
                    );
                    return res.ToLong();
                }
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

            if (nums.Count(x => x == '0') == last) return -1;

            var dic = nums.ToDic();

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

            //if (nums[last] != '0')
            {
                try
                {
                    var lhv = dic.Last(x =>
                        x.Chr != '0' && x.Ind != last &&
                        dic.FirstOrDefault(sx => sx.Ind > x.Ind && sx.Chr < x.Chr) != null);
                    var max = dic.Where(x => x.Ind > lhv.Ind && x.Chr < lhv.Chr).Max(x => x.Chr);
                    var rhv = dic.First(x => x.Ind > lhv.Ind && x.Chr == max);

                    var res = new List<char>(nums.Take(lhv.Ind)) {rhv.Chr};
                    res.AddRange(
                        dic.Where(x => x.Ind != rhv.Ind && x.Ind >= lhv.Ind)
                           .Select(x => x.Chr)
                           .OrderByDescending(x => x)
                    );

                    return res.ToLong();
                }
                catch
                {
                }
            }

            return -1;
        }
    }

    [TestClass]
    public class Tests
    {
        [TestMethod]
        [DataRow(1234567908, 1234567890)]
        [DataRow(1027, -1)]
        [DataRow(51226262651257, 51226262627551)]
        [DataRow(10829494039415810, 10829494039415801)]
        [DataRow(29009, 20990)]
        [DataRow(2071, 2017)]
        [DataRow(809, -1)]
        [DataRow(908, 890)]
        [DataRow(315, 153)]
        [DataRow(907, 790)]
        [DataRow(100, -1)]
        [DataRow(531, 513)]
        [DataRow(21, 12)]
        [DataRow(441, 414)]
        [DataRow(123456798, 123456789)]
        public void TestNextSmaller(long n, long exRes) 
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextSmaller(n));

        [TestMethod]
        [DataRow(51226262627551, 51226262627551)]
        [DataRow(2146095976, 2146096579)]
        [DataRow(12, 21)]
        [DataRow(1234567890, 1234567908)]
        [DataRow(1977075943, 1977079345)]
        [DataRow(12513, 12531)]
        [DataRow(2017, 2071)]
        [DataRow(20990, 29009)]
        [DataRow(7728322, 7732228)]
        [DataRow(1072, 1207)]
        [DataRow(125130, 125301)]
        [DataRow(125137, 125173)]
        [DataRow(125103, 125130)]
        [DataRow(153, 315)]
        [DataRow(890, 908)]
        [DataRow(980, -1)]
        [DataRow(100, -1)]
        [DataRow(12513513, 12513531)]
        [DataRow(414, 441)]
        [DataRow(513, 531)]
        [DataRow(790, 907)]
        [DataRow(123456789, 123456798)]
        [DataRow(10829494039415801, 10829494039415810)]
        public void TestNextBigger(long n, long exRes) 
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextBigger(n));
    }
}
