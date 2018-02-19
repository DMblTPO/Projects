using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUnitTests.Codewars;
using NUnit.Framework;

namespace MyUnitTests.Codewars
{
    public static class TimeFormat
    {
        public static string GetReadableTime(int seconds)
        {
            var hours = seconds / 3600;
            var mins = seconds / 60 % 60;
            var secs = seconds % 60;
            return $"{hours:D2}:{mins:D2}:{secs:D2}";
        }
    }
}

[TestFixture]
public class HumanReadableTimeTest
{
    [Test]
    public void HumanReadableTest()
    {
        Assert.AreEqual(TimeFormat.GetReadableTime(86399), "23:59:59");
        Assert.AreEqual(TimeFormat.GetReadableTime(0), "00:00:00");
        Assert.AreEqual(TimeFormat.GetReadableTime(5), "00:00:05");
        Assert.AreEqual(TimeFormat.GetReadableTime(60), "00:01:00");
        Assert.AreEqual(TimeFormat.GetReadableTime(359999), "99:59:59");
    }
}