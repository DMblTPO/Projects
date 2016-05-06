using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        internal class Address
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
        };

        IList<Address> getAddresses()
        {
            var db = new List<Address>
            {
                new Address {Address1 = "a1", Address2 = "a12", City = "c1", State = "s1", Zip = "z1"},
                new Address {Address1 = "a2", Address2 = "a22", City = "c2", State = "s2", Zip = "z2"},
                new Address {Address1 = "a3", Address2 = null,  City = "c3", State = "s3", Zip = null},
                new Address {Address1 = "a4", Address2 = "a42", City = "c4", State = "s4", Zip = "z4"},
                new Address {Address1 = "a5", Address2 = null,  City = "c5", State = "s5", Zip = "z5"}
            };

            return db;
        }

        [TestMethod]
        public void Test_LINQComparationOfNullOrEmptyStrings()
        {
            var data = getAddresses();

            var chk = new Address {Address1 = "a3", Address2 = null, City = "c3", State = "s3", Zip = null};

            var res = data.Where(x =>
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
    }
}
