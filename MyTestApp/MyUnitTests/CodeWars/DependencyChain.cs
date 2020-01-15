using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MyUnitTests.CodeWars
{
    public class TestTools
    {
        public static string Print(Dictionary<string, string[]> dir)
            => string.Join("\n", dir.Select(x => $"[{x.Key}] => {string.Join(",", x.Value)}"));

        public static bool? Match(Dictionary<string, string[]> ar, Dictionary<string, string[]> er)
        {
            string ToTxt(Dictionary<string, string[]> d) => string.Join(string.Empty,
                d.OrderBy(x => x.Key).Select(x => $"{x.Key}{string.Join(string.Empty, x.Value.OrderBy(xx => xx))}"));

            var sortedAr = ToTxt(ar);
            var sortedEr = ToTxt(er);

            return sortedEr.Equals(sortedAr);
        }
    }

    public class DependencyChain
    {
        public static Dictionary<string, string[]> ExpandDependencies1(Dictionary<string, string[]> dependencies)
        {
            var res = dependencies.ToDictionary(k => k.Key, v => v.Value.ToList());

            foreach (var d in dependencies)
            {
                foreach (var s in d.Value)
                {
                    GetAllDeps(s, d.Key);
                }
            }

            return res.ToDictionary(k => k.Key, v => v.Value.Distinct().ToArray());


            void GetAllDeps(string elem, string head)
            {
                if (elem.Equals(head))
                {
                    throw new InvalidOperationException($"A circular dependency for [{head}]");
                }
                var deps = dependencies[elem];
                if (deps.Length == 0)
                {
                    return;
                }
                foreach (var d in deps)
                {
                    GetAllDeps(d, head);
                }
                res[head].AddRange(deps);
            }
        }
        public static Dictionary<string, string[]> ExpandDependencies(Dictionary<string, string[]> dependencies)
        {
            var res = dependencies.ToDictionary(k => k.Key, v => new HashSet<string>(v.Value));

            foreach (var d in dependencies)
            {
                var lev1 = new Stack<string>(d.Value);

                while (lev1.Count != 0)
                {
                    var newDep = lev1.Pop();
                    if (d.Key.Equals(newDep))
                    {
                        throw new InvalidOperationException($"A circular dependency for [{d.Key}]");
                    }
                    res[d.Key].Add(newDep);
                    foreach (var s in res[newDep].Except(res[d.Key]))
                    {
                        lev1.Push(s);
                    }
                }
            }

            return res.ToDictionary(k => k.Key, v => v.Value.Distinct().ToArray());
        }
    }
}

namespace MyUnitTests.CodeWars
{
    [TestFixture]
    public class DependencyChainTest
    {
        [Test]
        public void ExampleFromDescription0()
        {
            // Arrange
            var startFiles = new Dictionary<string, string[]>
            {
                ["A"] = new[] { "B", "D" },
                ["B"] = new[] { "C" },
                ["C"] = new[] { "D" },
                ["D"] = new string[] { }
            };

            var correctFiles = new Dictionary<string, string[]>
            {
                ["A"] = new[] { "B", "C", "D" },
                ["B"] = new[] { "C", "D" },
                ["C"] = new[] { "D" },
                ["D"] = new string[] { }
            };

            // Act
            var actualFiles = DependencyChain.ExpandDependencies(startFiles);
            var errorMessage = "Expected:\n" + TestTools.Print(correctFiles) + "\nGot:\n" + TestTools.Print(actualFiles);
    
            // Assert
            Assert.IsTrue(TestTools.Match(actualFiles, correctFiles), errorMessage);
        }

        [Test]
        public void ExampleFromDescription()
        {
            // Arrange
            var startFiles = new Dictionary<string, string[]>
            {
                ["A"] = new[] { "B", "D" },
                ["B"] = new[] { "C" },
                ["B1"] = new[] { "C", "C11", "C2", "C7" },
                ["B2"] = new[] { "C" },
                ["B3"] = new[] { "C" },
                ["B4"] = new[] { "C" },
                ["B5"] = new[] { "C", "C10", "B8" },
                ["B6"] = new[] { "C" },
                ["B7"] = new[] { "C" },
                ["B8"] = new[] { "C" },
                ["B9"] = new[] { "C" },
                ["B10"] = new[] { "C" },
                ["C"] = new[] { "C1" },
                ["C1"] = new[] { "C2" },
                ["C2"] = new[] { "C3" },
                ["C3"] = new[] { "C4" },
                ["C4"] = new[] { "C5", "C7", "C9", "C11", "D" },
                ["C5"] = new[] { "C6" },
                ["C6"] = new[] { "C7" },
                ["C7"] = new[] { "C8" },
                ["C8"] = new[] { "C9" },
                ["C9"] = new[] { "C10" },
                ["C10"] = new[] { "D" },
                ["C11"] = new[] { "D" },
                ["C20"] = new[]
            {
                "C", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C21", "C22", "C23", "C24",
                "C25", "C26", "C27", "C28", "C29"
            },
                ["C21"] = new[] { "C22", "C23", "C24", "C25", "C26", "C27", "C28", "C29" },
                ["C22"] = new[] { "C23", "C24", "C25", "C26", "C27", "C28", "C29" },
                ["C23"] = new[] { "C24", "C25", "C26", "C27", "C28", "C29" },
                ["C24"] = new[] { "C25", "C26", "C27", "C28", "C29" },
                ["C25"] = new[] { "C26", "C27", "C28", "C29" },
                ["C26"] = new[] { "C27", "C28", "C29" },
                ["C27"] = new[] { "C28", "C29" },
                ["C28"] = new[] { "C29" },
                ["C29"] = new string[] { },
                ["D"] = new string[] { }
            };

            var correctFiles = new Dictionary<string, string[]>
            {
                ["A"] = new[]
                    {"B", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1", "C"},
                ["B"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B1"] = new[] {"C", "C11", "C2", "C7", "D", "C10", "C9", "C8", "C6", "C5", "C4", "C3", "C1"},
                ["B2"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B3"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B4"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B5"] = new[]
                    {"C", "C10", "B8", "D", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B6"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B7"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B8"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B9"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["B10"] = new[] {"C", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2", "C1"},
                ["C"] = new[] {"C1", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3", "C2"},
                ["C1"] = new[] {"C2", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4", "C3"},
                ["C2"] = new[] {"C3", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11", "C4"},
                ["C3"] = new[] {"C4", "D", "C10", "C9", "C8", "C7", "C6", "C5", "C11"},
                ["C4"] = new[] {"C5", "C7", "C9", "C11", "D", "C10", "C8", "C6"},
                ["C5"] = new[] {"C6", "D", "C10", "C9", "C8", "C7"},
                ["C6"] = new[] {"C7", "D", "C10", "C9", "C8"},
                ["C7"] = new[] {"C8", "D", "C10", "C9"},
                ["C8"] = new[] {"C9", "D", "C10"},
                ["C9"] = new[] {"C10", "D"},
                ["C10"] = new[] {"D"},
                ["C11"] = new[] {"D"},
                ["C20"] = new[]
                {
                    "C", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C21", "C22", "C23", "C24",
                    "C25", "C26", "C27", "C28", "C29", "D"
                },
                ["C21"] = new[] {"C22", "C23", "C24", "C25", "C26", "C27", "C28", "C29"},
                ["C22"] = new[] {"C23", "C24", "C25", "C26", "C27", "C28", "C29"},
                ["C23"] = new[] {"C24", "C25", "C26", "C27", "C28", "C29"},
                ["C24"] = new[] {"C25", "C26", "C27", "C28", "C29"},
                ["C25"] = new[] {"C26", "C27", "C28", "C29"},
                ["C26"] = new[] {"C27", "C28", "C29"},
                ["C27"] = new[] {"C28", "C29"},
                ["C28"] = new[] {"C29"},
                ["C29"] = new string[] { },
                ["D"] = new string[] { }
            };

            // Act
            var actualFiles = DependencyChain.ExpandDependencies(startFiles);
            var errorMessage = "Expected:\n" + TestTools.Print(correctFiles) + "\nGot:\n" +
                               TestTools.Print(actualFiles);

            // Assert
            Assert.IsTrue(TestTools.Match(actualFiles, correctFiles), errorMessage);
        }

        [Test]
        public void TestEmptyFileList()
        {
            // Arrange
            var startFiles = new Dictionary<string, string[]>();
            var correctFiles = new Dictionary<string, string[]>();

            // Act
            var actualFiles = DependencyChain.ExpandDependencies(startFiles);
            var errorMessage = "Expected:\n" + TestTools.Print(correctFiles) + "\nGot:\n" +
                               TestTools.Print(actualFiles);

            // Assert
            Assert.IsTrue(TestTools.Match(actualFiles, correctFiles), errorMessage);
        }

        [Test]
        public void TestCircularDependencies()
        {
            // Arrange
            var startFiles = new Dictionary<string, string[]>
            {
                ["A"] = new[] { "B" },
                ["B"] = new[] { "C" },
                ["C"] = new[] { "D" },
                ["D"] = new[] { "A" }
            };

            // Act/Assert
            Assert.Throws(typeof(InvalidOperationException),
                delegate { DependencyChain.ExpandDependencies(startFiles); },
                "A circular dependency should have thrown an InvalidOperationException.");
        }
    }
}

