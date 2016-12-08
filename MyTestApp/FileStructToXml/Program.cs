using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace FileStructToXml
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Usage: FileStructToXml <dir>");
                Console.ReadLine();
                return;
            }

            var dir = args[0];

            Console.OutputEncoding = Encoding.UTF8;

            var culUa = new CultureInfo("uk-UA");

            Thread.CurrentThread.CurrentCulture = culUa;
            Thread.CurrentThread.CurrentUICulture = culUa;

            int i = 0;

            if (dir[1] != ':')
            {
                dir = @".\" + dir;
            }

            RecurDir(dir, ref i);

            Console.ReadLine();
        }

        static void RecurDir(string fd, ref int i)
        {
            var tabset1 = new string(' ', i * 2);
            var tabset2 = new string(' ', i * 2 + 2);
            var sfd = fd.Split('\\');
            Console.WriteLine(@"{1}<Folder name='{0}'>", sfd.Last(), tabset1);
            foreach (var directory in Directory.GetDirectories(fd))
            {
                i++;
                RecurDir(directory, ref i);
                i--;
            }
            foreach (var file in Directory.GetFiles(fd))
            {
                Console.WriteLine(@"{1}<File name='{0}' />", file, tabset2);
            }
            Console.WriteLine(@"{0}</Folder>", tabset1);
        }
    }
}
