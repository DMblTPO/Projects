namespace MyUnitTests.CodeWars
{
    using NUnit.Framework;

    using Interval = System.ValueTuple<int, int>;

    public class IntervalTest
    {
        [Test]
        public void ShouldHandleEmptyIntervals()
        {
            Assert.AreEqual(0, Intervals.SumIntervals(new Interval[] { }));
            Assert.AreEqual(0, Intervals.SumIntervals(new Interval[] { (4, 4), (6, 6), (8, 8) }));
        }

        [Test]
        public void ShouldAddDisjoinedIntervals()
        {
            Assert.AreEqual(9, Intervals.SumIntervals(new Interval[] { (1, 2), (6, 10), (11, 15) }));
            Assert.AreEqual(11, Intervals.SumIntervals(new Interval[] { (4, 8), (9, 10), (15, 21) }));
            Assert.AreEqual(7, Intervals.SumIntervals(new Interval[] { (-1, 4), (-5, -3) }));
            Assert.AreEqual(78, Intervals.SumIntervals(new Interval[] { (-245, -218), (-194, -179), (-155, -119) }));
        }

        [Test]
        public void ShouldAddAdjacentIntervals()
        {
            Assert.AreEqual(54, Intervals.SumIntervals(new Interval[] { (1, 2), (2, 6), (6, 55) }));
            Assert.AreEqual(23, Intervals.SumIntervals(new Interval[] { (-2, -1), (-1, 0), (0, 21) }));
        }

        [Test]
        public void ShouldAddOverlappingIntervals()
        {
            Assert.AreEqual(19, Intervals.SumIntervals(new Interval[] { (1, 5), (10, 20), (1, 6), (16, 19), (5, 11) }));
            Assert.AreEqual(7, Intervals.SumIntervals(new Interval[] { (1, 4), (7, 10), (3, 5) }));
            Assert.AreEqual(6, Intervals.SumIntervals(new Interval[] { (5, 8), (3, 6), (1, 2) }));
        }

        [Test]
        public void ShouldHandleMixedIntervals()
        {
            Assert.AreEqual(13, Intervals.SumIntervals(new Interval[] { (2, 5), (-1, 2), (-40, -35), (6, 8) }));
            Assert.AreEqual(1234, Intervals.SumIntervals(new Interval[] { (-7, 8), (-2, 10), (5, 15), (2000, 3150), (-5400, -5338) }));
            Assert.AreEqual(158, Intervals.SumIntervals(new Interval[] { (-101, 24), (-35, 27), (27, 53), (-105, 20), (-36, 26) }));
        }

        [Test]
        public void ShouldHandleRandomIntervals()
        {
            Assert.AreEqual(18978, Intervals.SumIntervals(new Interval[] { (-3168, 3806), (-57, 9257), (3942, 4900), (5271, 5775), (-9721, -5120), (5020, 6854), (1023, 8878), (-2988, 7467), (-7040, 4150), (-6584, 873), (-5379, -4875), (-4566, -1062), (3171, 5921), (-5340, 8345), (-6894, -2), (-449, 7912), (-439, 761), (-9379, -2170)
            }));
        }
    }
}

namespace MyUnitTests.CodeWars
{
    using System.Collections.Generic;
    using System.Linq;

    public class Intervals
    {
        class Interval
        {
            public int X0 { get; set; }
            public int X1 { get; set; }
        }

        public static int SumIntervals((int, int)[] intervals)
        {
            if (intervals.Length == 0)
            {
                return 0;
            }

            var sum = new List<Interval>();
            var arr = intervals
                .Where(x => x.Item2 > x.Item1)
                .OrderBy(x => x.Item1)
                .GroupBy(x => x.Item1)
                .Select(x => new Interval
                {
                    X0 = x.Key,
                    X1 = x.Max(xx => xx.Item2),
                })
                .ToArray();

            if (arr.Length == 0)
            {
                return 0;
            }

            var a = arr[0];
            var start = a.X0;
            var end = a.X1;
            for (var i = 1; i < arr.Length; i++)
            {
                var b = arr[i];
                if (b.X1 > end)
                {
                    end = b.X1;
                }

                if (a.X1 >= b.X0)
                {
                    a = b;
                    continue;
                }

                sum.Add(new Interval {X0 = start, X1 = a.X1});
                start = b.X0;
                a = b;
            }

            sum.Add(new Interval {X0 = start, X1 = end});

            return sum.Sum(x => x.X1 - x.X0);
        }
    }
}