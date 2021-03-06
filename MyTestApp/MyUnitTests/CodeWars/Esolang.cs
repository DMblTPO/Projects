﻿using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MyUnitTests.CodeWars
{
    public class Esolang
    {
        public static string MyFirstInterpreter(string code)
        {
            var res = new StringBuilder(string.Empty);

            var chain = code
                .Split('.')
                .Select(x => x.Where(xx => xx=='+'))
                .Select(x => x.Count() % 256)
                .ToList();
            chain.RemoveAt(chain.Count-1);

            var i = 0;

            chain.ForEach(c =>
            {
                i += c;
                if (i > 255)
                {
                    i -= 256;
                }
                res.Append((char)i);
            });

            return res.ToString();
        }
    }
}

namespace MyUnitTests.CodeWars
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void SampleTests()
        {
            // Hello World Program - outputs the string "Hello, World!"
            Assert.AreEqual("Hello, World!", Esolang.MyFirstInterpreter("+++++++--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.+++++++++++++++++++++++++++++.+++++++..+++.+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.+++++++++++++++++++++++++++++++++++++++++++++++++++++++.++++++++++++++++++++++++.+++.++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++."));
            // Outputs the uppercase English alphabet
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Esolang.MyFirstInterpreter("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.+.+.+.+.+.+.+.+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+.+."));
        }
    }
}

namespace MyUnitTests.CodeWars
{
    using System.Text;
}
