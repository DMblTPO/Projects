using System.Collections.Generic;

namespace MyUnitTests
{
    internal class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
    };

    static partial class Data
    {
        public static IList<Address> Addresses
        {
            get
            {
                var db = new List<Address>
                {
                    new Address {Address1 = "a1", Address2 = "a12", City = "c1", State = "s1", Zip = "z1"},
                    new Address {Address1 = "a2", Address2 = "a22", City = "c2", State = "s2", Zip = "z2"},
                    new Address {Address1 = "a3", Address2 = null, City = "c3", State = "s3", Zip = null},
                    new Address {Address1 = "a4", Address2 = "a42", City = "c4", State = "s4", Zip = "z4"},
                    new Address {Address1 = "a5", Address2 = null, City = "c5", State = "s5", Zip = "z5"}
                };

                return db;
            }
        }
    }
}
