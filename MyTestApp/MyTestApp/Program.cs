using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyTestDbApp
{
    class MyDb
    {
        readonly string _conStr = @"Data Source=(local)\sqlexpress;Initial Catalog=qalight;Integrated Security=true";

        public void SendQuery(string sql)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                var sqlCmd = new SqlCommand(sql, connection) {CommandType = CommandType.Text};

                try
                {
                    connection.Open();
                    var reader = sqlCmd.ExecuteReader(/*CommandBehavior.KeyInfo*/);

                    var tbl = new DataTable();
                    tbl.Load(reader);

                    reader.Close();

                    var colLens = new List<int>();

                    var tmpContainer = new XElement("tr");
                    tbl.Columns
                        .Cast<DataColumn>()
                        .Select(c => new XElement("th", c.ColumnName))
                        .ToList()
                        .ForEach(x => tmpContainer.Add(x));

                    Console.Write("{0}", tmpContainer);

                    Console.WriteLine("");

                    tbl.Rows
                        .Cast<DataRow>()
                        .Select(r => r.ItemArray);

                    foreach (var r in tbl.Rows)
                    {
                        var row = r as DataRow;

                        var c = 0;
                        foreach (var o in row.ItemArray)
                        {
                            object f;
                            var fmt = "{0," + colLens[c++];
                            if (o is DateTime)
                            {
                                f = o.ToString().Substring(0, 10);
                                fmt += "}|";
                            }
                            else
                            {
                                f = o;
                                fmt += "} |";
                            }
                            Console.Write(fmt, f);
                        }
                        Console.WriteLine("");
                        
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    class Tests
    {
        public void Test1()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var culUA = new CultureInfo("uk-UA");

            Thread.CurrentThread.CurrentCulture = culUA;
            Thread.CurrentThread.CurrentUICulture = culUA;

            var q = new MyDb();
            q.SendQuery("select * from students");

            Console.ReadLine();
        }

        public void Test2()
        {
            // develop branch ini
            using (var timer = new System.Threading.Timer(JobFunc, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)))
            {
                Console.ReadKey();
            }
        }

        private void JobFunc(object state)
        {
            Console.Write($"\n[{DateTime.Now.TimeOfDay}] Job is started!\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tests = new Tests();

            Task.Run(async () =>
            {
                while (true)
                {
                    Console.Write(".");
                    await Task.Delay(1000);
                }
            });
            
            tests.Test2();
        }
    }
}
