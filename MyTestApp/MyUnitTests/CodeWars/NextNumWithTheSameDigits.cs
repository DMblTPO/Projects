using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUnitTests.CodeWars
{
    // http://www.codewars.com/kata/next-smaller-number-with-the-same-digits/train/csharp

    class Pair
    {
        public int Idx { get; set; }
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
            return n.ToString().ToArray();
        }

        public static IList<Pair> ToDic(this char[] arr)
        {
            return arr.Select((x, i) => new Pair {Idx = i, Chr = x}).ToList();
        }

        public static char[] SwapChar(this char[] arr, int p1, int p2)
        {
            var c = arr[p1];
            arr[p1] = arr[p2];
            arr[p2] = c;
            return arr;
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
                var lhv = dic.Last(x => x.Idx != last && dic.Any(sx => sx.Idx > x.Idx && sx.Chr > x.Chr));
                var rhv = dic.Last(x => x.Chr > lhv.Chr && x.Idx > lhv.Idx && x.Chr != '0');
                var res = new List<char>(nums.Take(lhv.Idx)) {rhv.Chr};
                res.AddRange(
                    dic.Where(x => x.Idx >= lhv.Idx && x.Idx != rhv.Idx && x.Idx != last + 1)
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
                                             sx => sx.Idx == x.Idx + 1 &&
                                                   sx.Chr > '0'
                                         ) != null);
                if (dic.Skip(zero.Idx).All(x => x.Chr <= dic[zero.Idx + 1].Chr))
                {
                    var ch = dic.Where(x => x.Idx > zero.Idx && x.Chr != '0').Min(x => x.Chr);
                    var replace = dic.First(x => x.Idx > zero.Idx && x.Chr == ch);
                    var res = new List<char>(nums.Take(zero.Idx)) {replace.Chr, '0'};
                    res.AddRange(
                        dic.Where(x => x.Idx > zero.Idx && x.Idx != replace.Idx)
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
            var lastIdx = nums.Length - 1;

            var dic = nums.ToDic();

            List<char> res;

            var rep = dic.Where(x => x.Idx != lastIdx).FirstOrDefault(x => x.Chr > nums[x.Idx+1]);
            var allZero = dic.Where(x => x.Chr == '0');
            var lastZero = allZero.LastOrDefault();
            var beforeZero = lastZero != null? dic.LastOrDefault(x => x.Idx < lastZero.Idx && x.Chr != '0'): null;
            var zeroOneSeq = allZero.Skip(1).All(x => allZero.Any(xx => xx.Idx == x.Idx - 1));

            if (beforeZero != null && 
                (lastIdx - allZero.Count() == beforeZero.Idx &&
                (dic.Any(x => x.Idx <= beforeZero.Idx-1 && x.Chr <= nums[x.Idx+1]) || beforeZero.Idx==0) ||
                 zeroOneSeq && nums[1] == '0' && 
                 dic.Where(x => x.Chr != '0').Skip(1).All(x => true)))
            {
                // 100, 12300, 123, 100023
                return -1;
            }

            if (allZero.Any())
            {
                if (lastZero.Idx == lastIdx && lastIdx > 1 && nums[lastIdx-1] != '0')
                {
                    // 12305670 -> 12305607
                    return nums.SwapChar(lastIdx, lastIdx - 1).ToLong();
                }
                if (lastZero != null && lastZero.Idx > 1 && lastZero.Idx != lastIdx)
                {
                    if (nums[lastZero.Idx-1] <= nums[lastZero.Idx+1])
                    {   // 3507 -> 3075
                        res = new List<char>(nums.Take(lastZero.Idx-1)){'0'};
                        res.AddRange(
                                new List<char>(nums.Skip(lastZero.Idx+1)){nums[lastZero.Idx-1]}.OrderByDescending(x => x)
                            );
                        return res.ToLong();
                    }
                    if (nums[lastZero.Idx-1] > nums[lastZero.Idx+1] && rep == null)
                    {   // 3705 -> 3570
                        res = new List<char>(nums.Take(lastZero.Idx-1)){nums[lastZero.Idx+1]};
                        res.AddRange(
                            new List<char>(nums.Skip(lastZero.Idx+2)){nums[lastZero.Idx-1],'0'}.OrderByDescending(x => x)
                        );
                        return res.ToLong();
                    }
                }
            }

            if (rep != null)
            {
                res = new List<char>(nums.Take(rep.Idx)){nums[rep.Idx+1]};
                res.AddRange(new List<char>(nums.Skip(rep.Idx+2)){nums[rep.Idx]}.OrderByDescending(x => x));
                return res.ToLong();
            }

            return -1;
        }
    }

    [TestClass]
    public class Tests
    {
        [TestMethod]
        [DataRow(1234567908, 1234567890)]
        [DataRow(100027, -1)]
        [DataRow(100, -1)]
        [DataRow(123, -1)]
        [DataRow(123000, -1)]
        [DataRow(10829494039415810, 10829494039415801)]
        [DataRow(92071, 92017)]
        [DataRow(4401, 4140)]
        [DataRow(441, 414)]
        [DataRow(2071, 2017)]
        [DataRow(29009, 20990)]
        [DataRow(809, -1)]
        [DataRow(908, 890)]
        [DataRow(315, 153)]
        [DataRow(907, 790)]
        [DataRow(100, -1)]
        [DataRow(531, 513)]
        [DataRow(21, 12)]
        [DataRow(123456798, 123456789)]
        [DataRow(51226262651257, 51226262627551)]
        public void TestNextSmaller(long n, long exRes)
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextSmaller(n));

        [TestMethod]
        [DataRow(51226262627551, 51226262627551)]
        //[DataRow(51226262627551, 51226262627551)]
        //[DataRow(2146095976, 2146096579)]
        //[DataRow(12, 21)]
        //[DataRow(1234567890, 1234567908)]
        //[DataRow(1977075943, 1977079345)]
        //[DataRow(12513, 12531)]
        //[DataRow(2017, 2071)]
        //[DataRow(20990, 29009)]
        //[DataRow(7728322, 7732228)]
        //[DataRow(1072, 1207)]
        //[DataRow(125130, 125301)]
        //[DataRow(125137, 125173)]
        //[DataRow(125103, 125130)]
        //[DataRow(153, 315)]
        //[DataRow(890, 908)]
        //[DataRow(980, -1)]
        //[DataRow(100, -1)]
        //[DataRow(12513513, 12513531)]
        //[DataRow(414, 441)]
        //[DataRow(513, 531)]
        //[DataRow(790, 907)]
        //[DataRow(123456789, 123456798)]
        //[DataRow(10829494039415801, 10829494039415810)]
        public void TestNextBigger(long n, long exRes)
            => Assert.AreEqual(exRes, NextNumWithTheSameDigits.NextBigger(n));
    }
}