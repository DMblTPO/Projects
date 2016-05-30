using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;

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

                    foreach (var c in tbl.Columns)
                    {
                        var column = c as DataColumn;
                        var cName = column.ColumnName;
                        var maxLen = column.MaxLength > 15 ? 15 : column.MaxLength;
                        var cLen = maxLen > cName.Length ? maxLen : cName.Length;
                        var fmt = "{0," + (++cLen) + "} |";

                        colLens.Add(cLen);

                        Console.Write(fmt, column);
                    }
                    Console.WriteLine("");

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

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; 
            
            var culUA = new CultureInfo("uk-UA");

            Thread.CurrentThread.CurrentCulture = culUA;
            Thread.CurrentThread.CurrentUICulture = culUA;

            var q = new MyDb();
            q.SendQuery("select * from students");

            Console.ReadLine();
        }
    }
}
