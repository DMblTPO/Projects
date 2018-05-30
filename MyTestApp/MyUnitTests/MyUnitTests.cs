using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUnitTests
{
    [TestClass]
    public class MyUnitTests
    {
        public MyUnitTests()
        {
            Debug.WriteLine("MyUnitTests ctor starts");
        }

        [TestInitialize]
        public void TestInitializer()
        {
            Debug.WriteLine("TestInitializer starts");
        }

        [TestMethod]
        public void TestFindBitLevel()
        {
            int FindBitLevel(int i)
            {
                for (int l = 1; l < 32; l++)
                {
                    if ((1 << l) > i)
                        return l;
                }

                return -1;
            }

            Assert.IsTrue(FindBitLevel(1000000) == 20);
            Assert.IsTrue(FindBitLevel(1) == 1);
            Assert.IsTrue(FindBitLevel(2) == 2);
            Assert.IsTrue(FindBitLevel(3) == 2);
            Assert.IsTrue(FindBitLevel(4) == 3);
            Assert.IsTrue(FindBitLevel(5) == 3);
            Assert.IsTrue(FindBitLevel(6) == 3);
            Assert.IsTrue(FindBitLevel(7) == 3);
            Assert.IsTrue(FindBitLevel(8) == 4);
            Assert.IsTrue(FindBitLevel(9) == 4);
            Assert.IsTrue(FindBitLevel(10) == 4);
            Assert.IsTrue(FindBitLevel(200) == 8);
            Assert.IsTrue(FindBitLevel(60000) == 16);
        }

        [TestMethod]
        public void Test_CompareNullOrEmptyStrings()
        {
            string s1 = string.Empty;
            string s2 = string.Empty;

            Assert.IsTrue(s1.Equals(s2, StringComparison.CurrentCultureIgnoreCase));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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


        [TestMethod]
        public void Test_Linq_Find_With_Null()
        {
            try
            {
                var flatCspCustomer = Data.IniCspCustomers
                    .Where(c => c.Domains != null);
                // .SelectMany(d => d.Domains, (p, c) => new {p.TenantId, c.Name});

                var toBeCreatedFlat = (
                    from c in Data.IniDbCustomers.Where(x => x.Comments.Contains("#2"))
                    join csp in flatCspCustomer on c.TenantId equals csp.TenantId
                    where
                        csp.Domains.Any(d => c.TenantName != null && d.Name.Equals(c.TenantName) ||
                                             c.PrimaryDomain != null && d.Name.Equals(c.PrimaryDomain))
                    select new
                    {
                        c.TenantId,
                        c.PrimaryDomain,
                        c.TenantName
                    }).ToList();

                Assert.IsTrue(toBeCreatedFlat.Count == 3);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
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

        [TestMethod]
        public async Task RunAsyncTestFactorial()
        {
            Console.WriteLine("Start  [{0}]", DateTime.Now);
            await AsyncTasks.DisplayResultAsync();
            Console.WriteLine("Finish [{0}]", DateTime.Now);
        }

        [TestMethod]
        public void CalcRecurFactorial()
        {
            var num = 10;
            Console.WriteLine("Factorial of {0} is {1}.", num, AsyncTasks.RecFact(num));
        }

        [TestMethod]
        public void ShowType()
        {
            Console.WriteLine(typeof(Data).ToString());
            Assert.AreEqual(typeof(Data).ToString(), "MyUnitTests.Data");
        }

        [TestMethod]
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

        [TestMethod]
        public void PublicAndStaticCtor()
        {
            Debug.WriteLine("PublicAndStaticCtor :: Begin");

            new SemiStaticClass();

            Debug.WriteLine("PublicAndStaticCtor :: End");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SelectWithPossibleNullValue()
        {
            List<int> listOfInts = new List<int> {1, 2, 3, 4, 5};

            var res = listOfInts.Select(i => (i%2 == 0 ? i.ToString() : null)).ToList();

            Assert.IsTrue(res.Count > 0);

            res = listOfInts.Where(i => i%2 == 0).Select(i => i.ToString()).ToList();

            Assert.IsTrue(res.Count > 0);
        }

        [TestMethod]
        public void NullableGuidToSting()
        {
            Guid? nullGuid = null;

            var res = string.IsNullOrEmpty(nullGuid.ToString());

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TotalDays()
        {
            var d1 = DateTime.Parse("2016-07-27");
            var d2 = DateTime.Parse("2016-07-23");

            var td = (d1 - d2).TotalDays + 1;

            Assert.IsTrue(td == 5d);
        }

        [TestMethod]
        public void TimeSpanForTheNextDay()
        {
            var t = DateTime.Today.AddDays(1d).AddHours(1d);

            Thread.Sleep(3000);

            var ct = DateTime.UtcNow;
            var ts = t - ct;

            Assert.IsTrue(ts > default(TimeSpan));
        }

        public class ScheduleTime
        {
            public class NextTs
            {
                public TimeSpan Time;
                public TimeSpan Remain;
            }

            private readonly TimeSpan _dueTime;
            private readonly TimeSpan _period;
            private readonly TimeSpan _endOfDay;
            private TimeSpan _lastNext;

            public ScheduleTime(TimeSpan dueTime, TimeSpan period)
            {
                _dueTime = dueTime;
                _period = period;
                _endOfDay = new TimeSpan(24, 0, 0);
                _lastNext = dueTime;
            }

            public NextTs NextRun(bool rightNow = false)
            {
                TimeSpan curTime = DateTime.UtcNow.TimeOfDay;

                if (rightNow)
                {
                    return new NextTs { Time = curTime, Remain = new TimeSpan(0, 0, 5) };
                }

                while (_lastNext < curTime)
                {
                    _lastNext += _period;
                }


                if (_lastNext > _endOfDay)
                {
                    return new NextTs { Time = _dueTime, Remain = _dueTime + _endOfDay - curTime };
                }

                return new NextTs {Time = _lastNext, Remain = _lastNext - curTime};
            }
        }


        [TestMethod]
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


        [TestMethod]
        public void DateToStringWithCulture()
        {
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var date = (new DateTime(2016, 09, 16)).ToString("d", culture);

            Debug.WriteLine(date);
            Assert.IsTrue(true);
        }
    }
}
