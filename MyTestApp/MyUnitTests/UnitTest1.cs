using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using AutoMapper;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

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

    [TestFixture()]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var accreditationDto = new AccreditationDto();
            var json = JsonConvert.SerializeObject(accreditationDto, Formatting.Indented);
            Debug.WriteLine(json);
        }

        [Test]
        public void TestMethod2()
        {
            for (var i = 0; i++ < 10;)
            {
                Debug.WriteLine($"{GeneratePassword(5)}");
                Thread.Sleep(50);
            }
        }

        string GeneratePassword(int len)
        {
            var r = new Random();
            var x = new[] { new { v = 'a', m = 25 }, new { v = 'A', m = 25 }, new { v = '0', m = 10 }, new { v = '!', m = 12 } };
            var f = new Func<int, char>((i) => (char)((byte)x[i].v + (byte)r.Next(x[i].m)));

            var s = new StringBuilder();
            s.Append($"{f(0)}{f(1)}{f(2)}{f(3)}");

            for (var i = 4; i++ < len; s.Append($"{f(r.Next(4))}")) { }

            return s.ToString();
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int? Foo { get; set; }
        }

        [Test]
        public void TestNullIgnore()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, Person>()
                    .ForAllMembers(opt => opt.Condition((s,d,m,m2) => m2 == null));
            });

            var mapper = config.CreateMapper();

            var sourcePerson = new Person
            {
                FirstName = "Bill",
                LastName = "Gates",
                Foo = null
            };

            var destinationPerson = new Person
            {
                FirstName = "James",
                //LastName = "",
                Foo = 1
            };

            mapper.Map(sourcePerson, destinationPerson);

            Assert.IsNotNull(destinationPerson);
            Assert.IsTrue(destinationPerson.Foo.Equals(1));
        }

        public class A
        {
            public int X { get; set; }
        }

        public class B
        {
            public int Id { get; set; }
            public A Ba { get; set; }
        }

        [Test]
        public void FindFirstPropInComplexType()
        {
            var type = typeof(B);

            var info1 = type.GetNestedProperty("Ba");

            var infos = new List<PropertyInfo>();

            type.GetNestedProperty2("Ba.X", infos);

            var parameter = Expression.Parameter(type, "p");
            var expression = parameter.BuildExpression(infos);
        }
    }

    public static class TypeEx2
    {
        public static void GetNestedProperty2(this Type t, string propName, IList<PropertyInfo> infos)
        {
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (properties.Count(p => p.Name == propName.Split('.')[0]) == 0)
                throw new ArgumentNullException($"Property {propName}, is not exists in object {t}");

            if (propName.Split('.').Length == 1)
            {
                infos.Add(t.GetProperty(propName));
                return;
            }

            if (infos.Count == 0)
            {
                infos.Add(t.GetProperty(propName.Split('.')[0]));
            }

            GetNestedProperty2(t.GetProperty(propName.Split('.')[0]).PropertyType, propName.Split('.')[1], infos);
        }

        public static MemberExpression BuildExpression(this Expression param, IList<PropertyInfo> infos)
        {
            MemberExpression me = Expression.Property(param, infos[0]);
            for (int i = 1; i < infos.Count; i++)
            {
                me = Expression.Property(me, infos[i]);
            }
            return me;
        }
    }

    public static class TypeEx
    {
        public static PropertyInfo GetNestedProperty(this Type t, string PropertName)
        {
            var properties = t.GetProperties();
            if (properties.Count(p => p.Name == PropertName.Split('.')[0]) == 0)
                throw new ArgumentNullException($"Property {PropertName}, is not exists in object {t}");

            if (PropertName.Split('.').Length == 1)
                return t.GetProperty(PropertName);

            return GetNestedProperty(t.GetProperty(PropertName.Split('.')[0]).PropertyType, PropertName.Split('.')[1]);
        }
    }
}
