using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class FactDecompTest
{
    private void Testing(int n, string expected) {
        Console.WriteLine("n: {0}, expected: {1}", n, expected);
        Assert.AreEqual(expected, FactDecomp.Decomp(n));
    }

    [TestMethod]
    public void Test() {
        Testing(17, "2^15 * 3^6 * 5^3 * 7^2 * 11 * 13 * 17");
        Testing(5, "2^3 * 3 * 5");
        Testing(22, "2^19 * 3^9 * 5^4 * 7^3 * 11^2 * 13 * 17 * 19");
        Testing(14, "2^11 * 3^5 * 5^2 * 7^2 * 11 * 13");
        Testing(25, "2^22 * 3^10 * 5^6 * 7^3 * 11^2 * 13 * 17 * 19 * 23");
    }
}

class FactDecomp
{
    private static Dictionary<int, int> map = new Dictionary<int, int>();

    private static void Devide(int i)
    {
        if (i <= 1)
        {
            return;
        }

        var pair = map.FirstOrDefault(x => i % x.Key == 0);

        if (pair.Key == 0)
        {
            map.Add(i, 1);
            return;
        }

        map[pair.Key]++;
        
        Devide(i / pair.Key);
    }

    public static string Decomp(int n)
    {
        map.Clear();
        map.Add(2, 1);

        for (int i = 3; i <= n; i++)
        {
            Devide(i);
        }

        var decomp = string.Join(" * ", map.Select(x => $"{x.Key}{(x.Value == 1 ? string.Empty : $"^{x.Value}")}"));

        return decomp;
    }
}