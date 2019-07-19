using NUnit.Framework;

namespace MyUnitTests.CodeWars
{
    public class Smallfuck
    {
        public static string Interpreter(string code, string tape)
        {
            var arr = tape.ToCharArray();
            var invert = new[] { '1', '0' };
            var i = 0;
            var loop = -1;
            for (var j = 0; j < code.Length; ++j)
            {
                var c = code[j];
                switch (c)
                {
                    case '*': arr[i] = invert[arr[i] - '0']; break;
                    case '<': --i; break;
                    case '>': ++i; break;
                    case '[': loop = j; break;
                    case ']': j = loop; break;
                    default: continue;
                }
                if (i < 0 || i == tape.Length)
                    break;
            }
            return new string(arr);
        }
    }
}

namespace MyUnitTests.CodeWars
{
    [TestFixture, Description("Your interpreter")]
    public class InterpreterTest
    {
        [Test, Description("should work for some example test cases")]
        public void ExampleTest()
        {
            // Flips the leftmost cell of the tape
            Assert.AreEqual("10101100", Smallfuck.Interpreter("*", "00101100"));
            // Flips the second and third cell of the tape
            Assert.AreEqual("01001100", Smallfuck.Interpreter(">*>*", "00101100"));
            // Flips all the bits in the tape
            Assert.AreEqual("11010011", Smallfuck.Interpreter("*>*>*>*>*>*>*>*", "00101100"));
            // Flips all the bits that are initialized to 0
            Assert.AreEqual("11111111", Smallfuck.Interpreter("*>*>>*>>>*>*", "00101100"));
            // Goes somewhere to the right of the tape and then flips all bits that are initialized to 1, progressing leftwards through the tape
            Assert.AreEqual("00000000", Smallfuck.Interpreter(">>>>>*<*<<*", "00101100"));
            Assert.AreEqual("11111111", Smallfuck.Interpreter("*[>*]", "00000000"));
        }
    }
}