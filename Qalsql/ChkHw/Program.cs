using System;
using System.IO;

namespace ChkHw
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        static void Main(string[] args)
        {
            using (var fs = File.OpenRead(@"c:\sql.sql"))
            {
                var automate = new HwAutomate(new StreamReader(fs));

                automate.Process();

                foreach (var pair in automate.Queries)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
            }

            Console.ReadKey();
        }
    }
}
