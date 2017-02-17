using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MyUnitTests
{
    // ReSharper disable InconsistentNaming
    public class AccreditationDto
    {
        public int Id { get; set; }
        public int InvestmentAccountId { get; set; }
        public int TypeId { get; set; }

        public DateTime? Date { get; set; }

        // individual investment account
        public bool Flag_0 { get; set; }
        public bool Flag_1 { get; set; }
        public bool Flag_2 { get; set; }
        public bool Flag_3 { get; set; }

        // entity investment account
        public bool Flag_4 { get; set; }
        public bool Flag_5 { get; set; }
        public bool Flag_6 { get; set; }
        public bool Flag_7 { get; set; }
        public bool Flag_8 { get; set; }
        public bool Flag_9 { get; set; }
        public bool Flag_10 { get; set; }
        public bool Flag_11 { get; set; }
        public bool Flag_12 { get; set; }
        public bool Flag_13 { get; set; }
        public bool Flag_14 { get; set; }
        public bool Flag_15 { get; set; }
        public bool Flag_16 { get; set; }
        public bool Flag_17 { get; set; }
        public bool Flag_18 { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var accreditationDto = new AccreditationDto();
            var json = JsonConvert.SerializeObject(accreditationDto, Formatting.Indented);
            Debug.WriteLine(json);
        }
    }
}
