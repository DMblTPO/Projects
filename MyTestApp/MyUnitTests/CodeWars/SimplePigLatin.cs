using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUnitTests.CodeWars
{
    [TestClass]
    public class SimplePigLatin
    {
        [TestMethod]
        public void KataTests()
        {
            Assert.AreEqual("igPay atinlay siay oolcay", Kata.PigIt("Pig latin is cool"));
            Assert.AreEqual("hisTay siay ymay tringsay", Kata.PigIt("This is my string"));
        }
    }

    public class Kata
    {
        static StringBuilder _sb = new StringBuilder();

        public static string PigIt(string str)
        {
            _sb.Clear();
            foreach (var s in str.Split(' '))
            {
                if (_sb.Length > 0) _sb.Append(" ");
                _sb.Append(s.Substring(1));
                _sb.Append(s[0]);
                _sb.Append("ay");
            }
            return _sb.ToString();
        }
    }
}