using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChkHw
{
    public enum HwState
    {
        Start = 0,
        Begin,
        End
    }

    public class HwTask
    {
        public int Lesson { get; set; }
        public int Task { get; set; }
    }

    public static class HwExtension
    {
        private static string[] ParseContent(this string line)
        {
            if (line.StartsWith("/*") && line.EndsWith("*/"))
            {
                var content = line.Split('*')[1];
                return content.Split('-');
            }
            return null;
        }
        public static bool IsBeginTask(this string line, ref HwTask task)
        {
            var parts = line.ParseContent();
            if (parts == null || parts.Length != 4 || !parts[3].Equals("begin"))
            {
                return false;
            }
            if (task != null)
            {
                task.Lesson = int.Parse(parts[0]);
                task.Task = int.Parse(parts[1]);
            }
            return true;
        }
        public static bool IsEndTask(this string line, HwTask task)
        {
            var parts = line.ParseContent();
            if (parts == null || parts.Length != 4 || !parts[3].Equals("end"))
            {
                return false;
            }
            if (task == null) throw new NullReferenceException("Did you forget HwTask?");
            if (task.Lesson == int.Parse(parts[0]) && 
                task.Task == int.Parse(parts[1]))
            {
                return true;
            }
            return false;
        }
    }

    public class HwAutomate
    {
        private TextReader _reader;
        private HwState _state;

        public HwAutomate(TextReader reader)
        {
            _reader = reader;
            _state = HwState.Start;
        }

        public async Task Process()
        {
            string line;
            var hwTask = new HwTask();
            while (true)
            {
                line = await _reader.ReadLineAsync();
                if (line.IsBeginTask(ref hwTask))
                {
                    Next();
                }
            }
        }

        private void Next()
        {
            switch (_state)
            {
                case HwState.Start:
                case HwState.Begin:
                    break;
                case HwState.End:
                    break;
            }
        }
    }

    public class HwChecker
    {
        public IList<Status> DoCheck(TextReader txt, int lessonId)
        {
            try
            {
                var state = HwState.Start;
                var queries = new List<string>();
                string query;
                while (true)
                {
                    string line = txt.ReadLine();

                    var task = new HwTask();

                    if ((state == HwState.Start || state == HwState.End ) && line.IsBeginTask(task))
                    {
                        state = HwState.Begin;

                        while(true)
                        {
                            line = txt.ReadLine();
                            if (state)
                            {
                                
                            }
                        }
                    }


                        do
                        {
                            if (line.StartsWith("/*") && line.EndsWith("-sql-end*/") && line)
                            {
                            }
                        } while (!string.Equals(line, $"/*{lesson}-{task}-sql-end*/", StringComparison.Ordinal));
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }

    public class Status
    {
        public bool Success { get; set; }
    }
}
