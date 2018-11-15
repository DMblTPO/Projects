using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUnitTests.CodeWars
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public static class MixingTests {

        [Test]
        public static void Test1() {
            Assert.AreEqual("2:eeeee/2:yy/=:hh/=:rr", Mixing.Mix("Are they here", "yes, they are here"));
            Assert.AreEqual("1:ooo/1:uuu/2:sss/=:nnn/1:ii/2:aa/2:dd/2:ee/=:gg", 
                Mixing.Mix("looping is fun but dangerous", "less dangerous than coding"));
            Assert.AreEqual("1:aaa/1:nnn/1:gg/2:ee/2:ff/2:ii/2:oo/2:rr/2:ss/2:tt", 
                Mixing.Mix(" In many languages", " there's a pair of functions"));
            Assert.AreEqual("1:ee/1:ll/1:oo", Mixing.Mix("Lords of the Fallen", "gamekult"));
            Assert.AreEqual("", Mixing.Mix("codewars", "codewars"));
            Assert.AreEqual("1:nnnnn/1:ooooo/1:tttt/1:eee/1:gg/1:ii/1:mm/=:rr", 
                Mixing.Mix("A generation must confront the looming ", "codewarrs"));
        }
    }

    public class Mixing 
    {
        public static string Mix(string s1, string s2)
        {
            Dictionary<char, int> ToDic(string s)
                => s.Where(x => x >= 'a' && x <= 'z')
                    .GroupBy(x => x)
                    .Select(x => new
                    {
                        x.Key,
                        Qty = x.Count()
                    })
                    .Where(x => x.Qty > 1)
                    .ToDictionary(k => k.Key, v => v.Qty);

            var d1 = ToDic(s1);
            var d2 = ToDic(s2);

            var sb = new Dictionary<char, (int i1, int i2)>();

            foreach (var p1 in d1)
            {
                sb.Add(p1.Key, (p1.Value, 0));
            }

            foreach (var p2 in d2)
            {
                if (sb.ContainsKey(p2.Key))
                {
                    sb[p2.Key] = (sb[p2.Key].i1, p2.Value);
                    continue;
                }

                sb.Add(p2.Key, (0, p2.Value));
            }

            return string.Join("/",
                sb.Select(x => new
                    {
                        x.Key,
                        Index = x.Value.i1 > x.Value.i2
                            ? 1
                            : x.Value.i1 < x.Value.i2
                                ? 2
                                : 3,
                        Value = x.Value.i1 > x.Value.i2 ? x.Value.i1 : x.Value.i2
                    })
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => x.Index)
                    .ThenBy(x => x.Key)
                    .Select(x => $"{(x.Index == 3? "=": $"{x.Index}")}:{new string(x.Key, x.Value)}"));
        }
    }
}