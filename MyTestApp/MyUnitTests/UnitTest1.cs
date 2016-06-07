using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUnitTests
{
    [TestClass]
    public class UnitTest1
    {
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
                        csp.Domains.Any(d => c.TenantName!=null && d.Name.Equals(c.TenantName) ||
                                             c.PrimaryDomain != null && d.Name.Equals(c.PrimaryDomain))
                    select new
                    {
                        c.TenantId, c.PrimaryDomain, c.TenantName
                    }).ToList();

                Assert.IsTrue(toBeCreatedFlat.Count == 3);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task TestCorrect() //note the return type of Task. This is required to get the async test 'waitable' by the framework
        {
            await Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Start at {0}", DateTime.Now);
                await Task.Delay(5000);
                Console.WriteLine("Done  at {0}", DateTime.Now);
            }).Unwrap(); //Note the call to Unwrap. This automatically attempts to find the most Inner `Task` in the return type.
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
    }
}
