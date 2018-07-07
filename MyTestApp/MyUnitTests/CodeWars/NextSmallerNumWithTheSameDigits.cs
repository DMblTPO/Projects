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

            if (n % MyPow(10, (nums.Length - 1)) == 0) return -1;

            var dic = nums.Select((x, i) => new {Pos = i, Value = x}).ToList();

            try
            {
                var zzz = new List<char>();
                var zero = dic.First(x => x.Value == '0');

                if (zero.Pos == nums.Length - 2)
                {
                    if (zero.Pos == 1 && nums[zero.Pos - 1] <= nums[zero.Pos + 1])
                    {
                        return -1;
                    }

                    zzz = nums.Take(zero.Pos - 1).ToList();
                    zzz.AddRange(
                        nums[zero.Pos - 1] > nums[zero.Pos + 1]
                            ? new[] {nums[zero.Pos + 1], nums[zero.Pos - 1], '0'}
                            : new[] {'0', nums[zero.Pos + 1], nums[zero.Pos - 1]});
                }
                else
                {
                    if (nums.Skip(zero.Pos + 1).All(x => x > nums[zero.Pos - 1]))
                    {
                        return -1;
                    }

                    zzz = nums.Take(zero.Pos - 1).ToList();
                    zzz.Add('0');
                    zzz.Add(dic[zero.Pos - 1].Value);
                    zzz.AddRange(nums.Skip(zzz.Count).OrderByDescending(x => x));
                }

                var zzzRes = zzz.ToLong();
                return zzzRes;
            }
            catch
            {
            }

            try
            {
                var lastSmall = dic.Last(x => dic.Take(x.Pos).Any(sx => sx.Value > x.Value));
                var lastBig = dic.Take(lastSmall.Pos).Last(x => x.Value > lastSmall.Value);

                var @new = nums.Take(lastBig.Pos).ToList();
                @new.Add(lastSmall.Value);
                @new.AddRange(
                    dic.Skip(lastBig.Pos)
                        .Where(x => x.Pos != lastSmall.Pos)
                        .Select(x => x.Value)
                        .OrderByDescending(x => x)
                );

                var res = @new.ToLong();

                return res;
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
        [DataRow(2071, 2017)]
        [DataRow(809, -1)]
        [DataRow(1027, -1)]
        [DataRow(908, 890)]
        [DataRow(1234567908, 1234567890)]
        [DataRow(315, 153)]
        [DataRow(29009, 20990)]
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
