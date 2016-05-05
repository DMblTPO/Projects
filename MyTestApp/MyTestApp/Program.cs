
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;

namespace MyTestApp
{
    class MyDb
    {
        readonly string _conStr = @"Data Source=(local)\sqlexpress;Initial Catalog=qalight;Integrated Security=true";

        public void SendQuery(string sql)
        {
            using (var connection = new SqlConnection(_conStr))
            {
                var sqlCmd = new SqlCommand(sql, connection);

                try
                {
                    connection.Open();
                    var reader = sqlCmd.ExecuteReader();

                    bool header = true;
                    while (reader.Read())
                    {
                        var fCnt = reader.FieldCount;
                        if (header)
                        {
                            for (int i = 0; i < fCnt; i++)
                            {
                                Console.Write("{0}|", reader.GetName(i));
                            }
                            Console.WriteLine("");
                            //Console.WriteLine("{0,3}|{1,20}|{2,20}|{3,3}|{4,7}|{5,3}|{6,15}|{7,12}|{8,5}"
                            //    , reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5), reader.GetName(6), reader.GetName(7), reader.GetName(8));
                            header = false;
                        }
                        for (int i = 0; i < fCnt; i++)
                        {
                            Console.Write("{0}|", reader[i]);
                        }
                        Console.WriteLine("");
                        //Console.WriteLine("{0,3}|{1,20}|{2,20}|{3,3}|{4,7}|{5,3}|{6,15}|{7,12}|{8,5}"
                        //    , reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6], reader[7], reader[8]);
                    }

                    reader.Close();
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
