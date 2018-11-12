using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace MyUnitTests.CodeWars
{
    using NUnit.Framework;

    [TestFixture]
    public class TestTimes
    {
        [Test]
        public void test1()
        {
            Assert.AreEqual("now", HumanTimeFormat.formatDuration(0));
        }

        [Test]
        public void test2()
        {
            Assert.AreEqual("1 second", HumanTimeFormat.formatDuration(1));
        }

        [Test]
        public void test3()
        {
            Assert.AreEqual("1 minute and 2 seconds", HumanTimeFormat.formatDuration(62));
        }

        [Test]
        public void test4()
        {
            Assert.AreEqual("2 minutes", HumanTimeFormat.formatDuration(120));
        }

        [Test]
        public void test5()
        {
            Assert.AreEqual("1 hour, 1 minute and 2 seconds", HumanTimeFormat.formatDuration(3662));
        }

        [Test]
        public void test6()
        {
            Assert.AreEqual("3 years, 7 days, 15 hours, 23 minutes and 12 seconds", HumanTimeFormat.formatDuration(95268192));
        }
    }

    public class HumanTimeFormat
    {
        public static string formatDuration(int seconds)
        {
            if (seconds <= 0)
            {
                return "now";
            }

            const long secs = 60;
            const long mins = secs * 60;
            const long hours = mins * 24;
            const long days = hours * 365;

            var daysLeft = seconds % days;
            var hoursLeft = daysLeft % hours;
            var minsLeft = hoursLeft % mins;

            var yy = seconds / days;
            var dd = daysLeft / hours;
            var hh = hoursLeft / mins;
            var mm = minsLeft / secs;
            var ss = minsLeft % secs;

            var list = new List<string>();

            string AddPlural(long x) => x > 1 ? "s" : string.Empty;

            void AddTimePart(long x, string name)
            {
                if(x > 0) list.Add($"{x} {name}{AddPlural(x)}");
            }

            AddTimePart(yy, "year");
            AddTimePart(dd, "day");
            AddTimePart(hh, "hour");
            AddTimePart(mm, "minute");
            AddTimePart(ss, "second");

            var count = list.Count;

            return count == 1 ? list.First() : $"{string.Join(", ", list.Take(count - 1))} and {list.Last()}";
        }
    }
}