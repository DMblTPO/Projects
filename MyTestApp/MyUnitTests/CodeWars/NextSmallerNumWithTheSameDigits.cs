using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MyUnitTests.CodeWars
{
    // http://www.codewars.com/kata/next-smaller-number-with-the-same-digits/train/csharp

    class NextSmallerNumWithTheSameDigits
    {
        private static void Swap(char[] arr, int x, int y)
        {
            var z = arr[x];
            arr[x] = arr[y];
            arr[y] = z;
        }

        private static void Swap(Dictionary<int, char> dic, int x, int y)
        {
            var z = dic[x];
            dic[x] = dic[y];
            dic[y] = z;
        }

        public static long NextSmaller(long n)
        {
            if (n < 21) return -1;

            var nums = n.ToString().Select(x => x).ToArray();
            
            var dic = nums.Select((x, i) => new {Key = i, Value = x}).ToList(); //.ToDictionary(x => x.key, x => x.value);

            var len = nums.Length;
            var last = len - 1;

            if (nums[last] == '0' && n % (10 ^ len-1) == 0)
            {
                return -1;
            }

            if (nums[last] == '0')
            {
                for (var i = last-1; i > 0; i--)
                {
                    if (nums[i] > '0')
                    {
                        Swap(nums, i, i + 1);
                        return long.Parse(new string(nums));
                    }
                }
            }

            var lastSmall = dic.LastOrDefault(x => dic.Take(x.Key).Any(sx => sx.Value > x.Value));
            var lastBig = dic.Take(lastSmall.Key).LastOrDefault(x => x.Value > lastSmall.Value);

            var @new = dic.Take(lastBig.Key).ToList();
            @new.Add(lastSmall);
            @new.AddRange(dic.Skip(lastBig.Key + 1).Take(lastSmall.Key - lastBig.Key - 1).OrderByDescending(x => x.Value));

            var s = last;
            for (var k = last; k > 0; k--)
            {
                if (nums[k - 1] > nums[k])
                {
                    s = k - 1;
                    break;
                }
            }

            for (var k = s; k > 0; k--)
            {
                Swap(nums, k, k - 1);
                if (nums[k] > nums[k - 1])
                {
                    if (nums[0] == '0') return -1;
                    return long.Parse(new string(nums));
                }
            }

            return -1;

        }
    }

    [TestFixture]
    public class Tests
    {
        [TestCase(29009, ExpectedResult = 20990)]
        [TestCase(100, ExpectedResult = -1)]
        [TestCase(907, ExpectedResult = 790)]
        [TestCase(21, ExpectedResult = 12)]
        [TestCase(531, ExpectedResult = 513)]
        [TestCase(1027, ExpectedResult = -1)]
        [TestCase(441, ExpectedResult = 414)]
        [TestCase(123456798, ExpectedResult = 123456789)]
        public long FixedTests(long n)
        {
            return NextSmallerNumWithTheSameDigits.NextSmaller(n);
        }
    }
}
