using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MyUnitTests
{
    [TestFixture]
    public class CodeWars
    {
        static IList<string> DirReduceList(IList<string> list, int cur)
        {
            if (list.Count - cur < 2)
            {
                return list;
            }
            var x = cur; var y = cur + 1;
            if (list[x].Equals("NORTH") && list[y].Equals("SOUTH") ||
                list[x].Equals("SOUTH") && list[y].Equals("NORTH") ||
                list[x].Equals("EAST") && list[y].Equals("WEST") ||
                list[x].Equals("WEST") && list[y].Equals("EAST"))
            {
                list.RemoveAt(cur);
                list.RemoveAt(cur);
                return DirReduceList(list, 0);
            }
            return DirReduceList(list, cur + 1);
        }

        static string[] dirReduc(string[] arr)
        {
            return DirReduceList(arr.ToList(), 0).ToArray();
        }

        [Test]
        public void Test1()
        {
            string[] a = new string[] { "NORTH", "SOUTH", "SOUTH", "EAST", "WEST", "NORTH", "WEST" };
            string[] b = new string[] { "WEST" };
            Assert.AreEqual(b, dirReduc(a));
        }
        [Test]
        public void Test2()
        {
            string[] a = new string[] { "NORTH", "WEST", "SOUTH", "EAST" };
            string[] b = new string[] { "NORTH", "WEST", "SOUTH", "EAST" };
            Assert.AreEqual(b, dirReduc(a));
        }
    }
}
