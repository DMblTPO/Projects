using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MyUnitTests.CodeWars
{
    public class BurrowsWheelerTransformation
    {
        private static bool Check(string s)
            => s.Distinct().Count() <= 1 ? true : false;

        public static Tuple<string, int> Encode(string s)
        {
            if (Check(s)) return new Tuple<string, int>(s, 0);

            var m = new List<string>{s};
            var len = s.Length;
            for (var i = 1; i < len; i++)
                m.Add($"{m[i-1][len-1]}{m[i-1].Substring(0, len-1)}");
            var sort = m.OrderBy(x => x, StringComparer.Ordinal).Select((x, j) => new {Idx = j, Str = x}).ToArray();
            var index = sort.Single(x => x.Str == s);
            var encodeStr = string.Join("", sort.Select(x => x.Str.Last()));
            return new Tuple<string, int>(encodeStr, index.Idx);
        }

        public static string Decode(string s, int idx)
        {
            if (Check(s)) return s;
            var cn = s.Select(x => x.ToString()).ToArray();
            var cl = cn
                .Select((x, i0) => new {x, i0})
                .GroupBy(x => x.x, StringComparer.Ordinal)
                .SelectMany(g => g.Select((x, i) => new {x.x, x.i0, i}))
                .ToArray();
            var cf = cn
                .OrderBy(x => x, StringComparer.Ordinal)
                .Select((x, i0) => new {x, i0})
                .GroupBy(x => x.x, StringComparer.Ordinal)
                .SelectMany(g => g.Select((x, i1) => new {x.x, x.i0, i = i1}))
                .ToArray();
            var c = cf[idx];
            var buf = new StringBuilder(c.x);
            for (var i = 1; i < cn.Length; i++)
            {
                var i0 = cl.Single(z => z.x.Equals(c.x, StringComparison.Ordinal) && z.i == c.i).i0;
                c = cf.Single(z => z.i0 == i0);
                buf.Append(c.x);
            }
            return buf.ToString();
        }
    }
}

namespace MyUnitTests.CodeWars
{
    using NUnit.Framework;
    using Trans = BurrowsWheelerTransformation;

    [TestFixture]
    public class BurrowsWheelerTransformationSolutionTest
    {
        [Test]
        public void EncodingTest()
        {
            var encode = Trans.Encode("");
            StringAssert.AreEqualIgnoringCase("caab", encode.Item1);
            Assert.AreEqual(1, encode.Item2);

            encode = Trans.Encode("bananabar");
            StringAssert.AreEqualIgnoringCase("nnbbraaaa", encode.Item1);
            Assert.AreEqual(4, encode.Item2);
            
            encode = Trans.Encode("Humble Bundle");
            StringAssert.AreEqualIgnoringCase("e emnllbduuHB", encode.Item1);
            Assert.AreEqual(2, encode.Item2);

            encode = Trans.Encode("Mellow Yellow");
            StringAssert.AreEqualIgnoringCase("ww MYeelllloo", encode.Item1);
            Assert.AreEqual(1, encode.Item2);
        }

        [Test]
        public void DecodingTest()
        {
            StringAssert.AreEqualIgnoringCase("bananabar", Trans.Decode("nnbbraaaa", 4));
            StringAssert.AreEqualIgnoringCase("Humble Bundle", Trans.Decode("e emnllbduuHB", 2));
            StringAssert.AreEqualIgnoringCase("Mellow Yellow", Trans.Decode("ww MYeelllloo", 1));
            StringAssert.AreEqualIgnoringCase(new string('x', 20), Trans.Decode(new string('x', 20), 0));
        }
    }
}