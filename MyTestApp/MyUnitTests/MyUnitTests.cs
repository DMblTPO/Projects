using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyUnitTests
{
    [TestFixture()]
    public class MyUnitTests
    {
        public MyUnitTests()
        {
            Debug.WriteLine("MyUnitTests ctor starts");
        }

        [SetUp]
        public void TestInitializer()
        {
            Debug.WriteLine("TestInitializer starts");
        }

        [Test]
        public void Test_CompareNullOrEmptyStrings()
        {
            string s1 = string.Empty;
            string s2 = string.Empty;

            Assert.IsTrue(s1.Equals(s2, StringComparison.CurrentCultureIgnoreCase));
        }

        [Test]
        public void Test_LINQComparationOfNullOrEmptyStrings()
        {
            var chk = new Address {Address1 = "a3", Address2 = null, City = "c3", State = "s3", Zip = null};

            var res = Data.Addresses.Where(x =>
                string.Format("{0}|{1}|{2}|{3}|{4}", x.Address1, x.Address2, x.City, x.State, x.Zip).Equals(
                    string.Format("{0}|{1}|{2}|{3}|{4}", chk.Address1, chk.Address2, chk.City, chk.State, chk.Zip)
                    ))
                .Select(x => new
                {
                    a = x.Address1,
                    b = x.City
                });

            Assert.AreEqual(res.Count(), 1);
        }

        [Test]
        public void Test_Aggregate()
        {
            string sentence = "the quick brown fox jumps over the lazy dog";

            // Split the string into individual words.
            string[] words = sentence.Split(' ');

            var glueBack = words.Aggregate((first, next) =>
                first + " " + next);

            // Prepend each word to the beginning of the 
            // new sentence to reverse the word order.
            string reversed = words.Aggregate((first, next) =>
                next + " " + first);

            Debug.WriteLine(sentence);
            Debug.WriteLine(glueBack);

            Assert.AreEqual(sentence, glueBack);

            Debug.WriteLine(reversed);
            Debug.WriteLine(new String(sentence.Reverse().ToArray()));

            Assert.AreEqual(reversed.Length, sentence.Length);
        }

        [Test]
        public void Test_Aggregate_Guid()
        {
            List<Guid> lg = new List<Guid>
            {
                Guid.Parse("3C8B2B93-7ABB-42DD-9912-6758562DB47A"),
                Guid.Parse("B08035EE-B1DE-457D-8D8E-8D293D303C6D"),
                Guid.Parse("0A6059FC-0BFD-408F-8D2A-2E0A1AD69ACB")
            };

            var glueBack = lg.Select(g => string.Format("'{0}'", g)).Aggregate((first, next) =>
                first + ", " + next);

            Assert.IsNotNull(glueBack);

            Debug.WriteLine(glueBack);
        }


        [Test]
        public async Task TestCorrect()
            //note the return type of Task. This is required to get the async test 'waitable' by the framework
        {
            await Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Start at {0}", DateTime.Now);
                await Task.Delay(5000);
                Console.WriteLine("Done  at {0}", DateTime.Now);
            }).Unwrap();
                //Note the call to Unwrap. This automatically attempts to find the most Inner `Task` in the return type.
            Console.WriteLine("All done [{0}]", DateTime.Now);
        }

        [Test]
        public void ShowType()
        {
            Console.WriteLine(typeof(Data).ToString());
            Assert.AreEqual(typeof(Data).ToString(), "MyUnitTests.Data");
        }

        [Test]
        public void TryParseBoolean()
        {
            bool bFlag;
            var bOk = bool.TryParse("true", out bFlag);

            Assert.IsTrue(bFlag && bOk);

            bOk = bool.TryParse("True", out bFlag);

            Assert.IsTrue(bFlag && bOk);

            bOk = bool.TryParse("tru", out bFlag);

            Assert.IsTrue(!bFlag && !bOk);

            bOk = bool.TryParse("false", out bFlag);

            Assert.IsTrue(!bFlag && bOk);

            bOk = bool.TryParse(null, out bFlag);

            Assert.IsTrue(!bFlag && !bOk);
        }

        [Test]
        public void PublicAndStaticCtor()
        {
            Debug.WriteLine("PublicAndStaticCtor :: Begin");

            new SemiStaticClass();

            Debug.WriteLine("PublicAndStaticCtor :: End");

            Assert.IsTrue(true);
        }

        [Test]
        public void SelectWithPossibleNullValue()
        {
            List<int> listOfInts = new List<int> {1, 2, 3, 4, 5};

            var res = listOfInts.Select(i => (i%2 == 0 ? i.ToString() : null)).ToList();

            Assert.IsTrue(res.Count > 0);

            res = listOfInts.Where(i => i%2 == 0).Select(i => i.ToString()).ToList();

            Assert.IsTrue(res.Count > 0);
        }

        [Test]
        public void NullableGuidToSting()
        {
            Guid? nullGuid = null;

            var res = string.IsNullOrEmpty(nullGuid.ToString());

            Assert.IsTrue(res);
        }

        [Test]
        public void TotalDays()
        {
            var d1 = DateTime.Parse("2016-07-27");
            var d2 = DateTime.Parse("2016-07-23");

            var td = (d1 - d2).TotalDays + 1;

            Assert.IsTrue(td == 5d);
        }

        [Test]
        public void TimeSpanForTheNextDay()
        {
            var t = DateTime.Today.AddDays(1d).AddHours(1d);

            Thread.Sleep(3000);

            var ct = DateTime.UtcNow;
            var ts = t - ct;

            Assert.IsTrue(ts > default(TimeSpan));
        }


        [Test]
        public void GetNextTime()
        {
            var dueTime = new TimeSpan(14, 0, 0);
            var period = new TimeSpan(3, 0, 0);

            var sch = new ScheduleTime(dueTime, period);
            var ts = sch.NextRun(true);

            Debug.WriteLine($"Now: {DateTime.UtcNow.TimeOfDay}");
            Debug.WriteLine($"What the next: {ts.Time}");
            Debug.WriteLine($"How much to the next: {ts.Remain}");

            ts = sch.NextRun();

            Debug.WriteLine($"Now: {DateTime.UtcNow.TimeOfDay}");
            Debug.WriteLine($"What the next: {ts.Time}");
            Debug.WriteLine($"How much to the next: {ts.Remain}");

            Assert.IsTrue(true);
        }
        
        [Test]
        public void DateToStringWithCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var date = (new DateTime(2016, 09, 16)).ToString("d", culture);

            Debug.WriteLine(date);
            Assert.IsTrue(true);
        }

        [Test]
        public void HowMuchTimeToNextStart()
        {
            var startAt = TimeSpan.Parse("11:00:00");

            var utcNow = DateTime.UtcNow;
            var curDate = utcNow.Date;
            var curTime = utcNow.TimeOfDay;

            var nextDateTime = curDate.Add(startAt);
            if (startAt < curTime)
            {
                nextDateTime = nextDateTime.AddDays(1);
            }

            var timeToStart = nextDateTime - utcNow;

            Debug.WriteLine(timeToStart);
            Assert.IsTrue(true);
        }

        [Test]
        public void ProcentFormat()
        {
            Debug.WriteLine($"({(-0.5678d).ToString("P", new CultureInfo("en-US"))})");
        }

        [Test]
        public void RegExTest()
        {
            var regex = @"^\d+\.?\d{0,2}$";

            var match = Regex.Match(string.Format("{0}", decimal.Parse("1.01")), regex, RegexOptions.IgnoreCase);

            Assert.IsTrue(match.Success);
        }

        [Test]
        public void GroupJoinTest()
        {
            var off = new[]
            {
                new { id = 1, title = "offering-1", typ = 'e' },
                new { id = 2, title = "offering-2", typ = 'd' },
                new { id = 3, title = "offering-3", typ = 'e' },
            };

            var inv = new[]
            {
                new { id = 1, dt = DateTime.Today.AddDays(-3), amount =   204m, offid = 2 },
                new { id = 2, dt = DateTime.Today.AddDays(-2), amount =   134m, offid = 1 },
                new { id = 3, dt = DateTime.Today.AddDays(-1), amount =    56m, offid = 1 },
                new { id = 4, dt = DateTime.Today.AddDays(-4), amount = 12.34m, offid = 3 },
                new { id = 5, dt = DateTime.Today.AddDays(-2), amount =    43m, offid = 1 },
                new { id = 6, dt = DateTime.Today.AddDays(-1), amount =    70m, offid = 2 },
            };

            var groupJoin = off.GroupJoin(
                inv,
                o => o.id,
                i => i.offid,
                (o, ii) => new
                {
                    id = o.id,
                    title = o.title,
                    typ = o.typ,
                    total = ii.Sum(x => x.amount)
                }
            );

            foreach (var x1 in groupJoin)
            {
                Debug.WriteLine(x1);
            }
        }

        [Flags]
        public enum Providers
        {
            None = 1,
            Google = 2,
            LinkedIn = 4
        }

        [Test]
        public void TestEnumXOR()
        {
            var p = Providers.None;

            Debug.WriteLine($"1) Ini: {p}");

            p |= Providers.Google;
            p |= Providers.LinkedIn;

            Debug.WriteLine($"2) Add Google + LinkedIn: {p}");

            p ^= Providers.Google;

            Debug.WriteLine($"3) Remove Google: {p}");
        }
    }
}
