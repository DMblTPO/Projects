using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MyUnitTests.Codewars
{
    public class BreadcrumbGenerator
    {
        private static string ToAcronym(string s)
        {
            var ss = s.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length <= 30)
                return string.Join(" ", ss).ToUpper();
            return string
                .Concat(ss.Where(x =>
                        !new[] {"the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a"}
                            .Contains(x))
                    .Select(x => x[0])).ToUpper();
        }

        public static string GenerateBC(string url, string separator)
        {
            const string home = "HOME";
            const string fmtA = "<a href=\"{0}\">{1}</a>";
            const string fmtSpan = "<span class=\"active\">{0}</span>";
            
            var crumbs = url.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var length = crumbs.Length;
            
            var isHttpHere = crumbs[0].StartsWith("http");
            var isLastIndex = crumbs[length - 1].StartsWith("index.");
            var isLastTag = crumbs[length - 1].StartsWith("#");
            var isHomeOnly = length == 1 || 
                             length == 2 && (isHttpHere || isLastIndex || isLastTag) || 
                             length == 3 && isHttpHere && isLastIndex ||
                             length == 3 && isHttpHere && isLastTag;

            if (isHomeOnly)
                return string.Format(fmtSpan, home);

            var startPos = isHttpHere ? 2 : 1;
            var finishPos = isLastIndex || isLastTag ? length - 2 : length - 1;

            var bc = new StringBuilder(string.Format(fmtA, "/", home));
            bc.Append(separator);

            var c = string.Empty;
            var href = new StringBuilder();
            for (int i = startPos; i < finishPos; i++)
            {
                c = crumbs[i];
                href.Append($"/{c}");
            }

            var sHref = href.ToString();
            if (!string.IsNullOrEmpty(sHref))
            {
                bc.Append(string.Format(fmtA, $"{href}/", ToAcronym(c)));
                bc.Append(separator);
            }
            bc.Append(string.Format(fmtSpan, ToAcronym(crumbs[finishPos].Split('.', '?')[0])));
            return bc.ToString();
        }

    }
}

namespace MyUnitTests.Codewars
{
    [TestFixture]
    public class SolutionTest
    {
        private string[] urls = {"mysite.com/pictures/holidays.html",
                                          "www.codewars.com/users/GiacomoSorbi?ref=CodeWars",
                                          "www.microsoft.com/docs/index.htm#top",
                                          "mysite.com/very-long-url-to-make-a-silly-yet-meaningful-example/example.asp",
                                          "www.very-long-site_name-to-make-a-silly-yet-meaningful-example.com/users/giacomo-sorbi",
                                          "https://www.linkedin.com/in/giacomosorbi",
                                          "www.agcpartners.co.uk/",
                                          "www.agcpartners.co.uk",
                                          "https://www.agcpartners.co.uk/index.html",
                                          "http://www.agcpartners.co.uk"};

        private string[] seps = { " : ", " / ", " * ", " > ", " + ", " * ", " * ", " # ", " >>> ", " % " };


        private string[] anss = {"<a href=\"/\">HOME</a> : <a href=\"/pictures/\">PICTURES</a> : <span class=\"active\">HOLIDAYS</span>",
                                          "<a href=\"/\">HOME</a> / <a href=\"/users/\">USERS</a> / <span class=\"active\">GIACOMOSORBI</span>",
                                          "<a href=\"/\">HOME</a> * <span class=\"active\">DOCS</span>",
                                          "<a href=\"/\">HOME</a> > <a href=\"/very-long-url-to-make-a-silly-yet-meaningful-example/\">VLUMSYME</a> > <span class=\"active\">EXAMPLE</span>",
                                          "<a href=\"/\">HOME</a> + <a href=\"/users/\">USERS</a> + <span class=\"active\">GIACOMO SORBI</span>",
                                          "<a href=\"/\">HOME</a> * <a href=\"/in/\">IN</a> * <span class=\"active\">GIACOMOSORBI</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>",
                                          "<span class=\"active\">HOME</span>"};
        [Test]
        public void ExampleTests()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\nTest With: {urls[i]}");
                if (i == 5) Console.WriteLine("\nThe one used in the above test was my LinkedIn Profile; if you solved the kata this far and manage to get it, feel free to add me as a contact, message me about the language that you used and I will be glad to endorse you in that skill and possibly many others :)\n\n ");

                Assert.AreEqual(anss[i], BreadcrumbGenerator.GenerateBC(urls[i], seps[i]));
            }
        }
    }
}