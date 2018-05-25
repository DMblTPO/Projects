using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using NUnit.Framework;
using static System.Math;

namespace MyUnitTests.Codewars
{
    public static class Kata
    {
        // https://www.codewars.com/kata/number-of-trailing-zeros-of-n/train/csharp
        public static int TrailingZeros(int n)
        {
            var ret = 0;

            for (var i = 5; n >= i; i += 5)
            {
                var s = i.ToString();
                var len = s.Length - 1;
                var pow = (int) Pow(10, len);

                if (len > 0 && i % pow == 0)
                {
                    ret += len;
                    if (s[0] == '5') ++ret;
                    continue;
                }

                if (i % 25 == 0  && i % 125 != 0 && (i - 250) % 125 != 0)
                {
                    ret += 2;
                }
                else
                if (i % 125 == 0 || (i - 250) % 125 == 0)
                {
                    ret += 3;
                }
                else
                {
                    ++ret;
                }

                for (var j = 10; j < n; j*=10)
                {
                    if (n % j == 0) ++ret;
                }
            }

            return ret;
        }
    }

    [TestFixture]
    public class TrailingZerosTest 
    {
        [Test]
        public void BasicTests() 
        {
            Assert.AreEqual(1, Kata.TrailingZeros(5));
            Assert.AreEqual(2, Kata.TrailingZeros(10));
            Assert.AreEqual(6, Kata.TrailingZeros(25));
            Assert.AreEqual(10, Kata.TrailingZeros(47));
            Assert.AreEqual(38, Kata.TrailingZeros(156));
            Assert.AreEqual(69, Kata.TrailingZeros(283));
            Assert.AreEqual(131, Kata.TrailingZeros(531));
        }
    }
}

