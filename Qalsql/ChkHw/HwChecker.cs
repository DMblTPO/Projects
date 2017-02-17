using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChkHw
{
    public enum HwState
    {
        Start = 0,
        Begin,
        End,
        Skip,
        Query
    }

    public class HwTask
    {
        public int Lesson { get; set; }
        public int Task { get; set; }
    }


    public static class HwExtension
    {
        private static RegexOptions TmpOptions => RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant;
        private static string TmpBegin => @"^\/\*sql-begin-\d{1,2}-\d{1,2}\*\/$";
        private static string TmpEnd => @"^\/\*sql-end-\d{1,2}-\d{1,2}\*\/$";
        private static string TmpSkip => @"(^\s*--)|(^\s*\/\*[^s]+.*\*\/)|(^\s*\/\*\s*\*\/)";
        private static string TmpSplit => @"\d{1,2}";

        private static bool IsBegin(this string line)
        {
            return Regex.IsMatch(line, TmpBegin, TmpOptions);
        }

        private static bool IsEnd(this string line)
        {
            return Regex.IsMatch(line, TmpEnd, TmpOptions);
        }

        private static bool ToSkip(this string line)
        {
            return Regex.IsMatch(line, TmpSkip, TmpOptions);
        }

        public static HwState LineState(this string line)
        {
            if (line.ToSkip())
            {
                return HwState.Skip;
            }
            if (line.IsBegin())
            {
                return HwState.Begin;
            }
            if (line.IsEnd())
            {
                return HwState.End;
            }
            return HwState.Query;
        }

        public static HwTask ParseNumbers(this string line)
        {
            try
            {
                Func<Match, int> asInt = m => int.Parse(m.ToString());
                var nums = new Regex(TmpSplit, TmpOptions).Matches(line);
                return new HwTask{ Lesson = asInt(nums[0]), Task = asInt(nums[1]) };
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }

    public class HwAutomate
    {
        private readonly TextReader _reader;
        private HwState _stateProcess;
        private string _line = string.Empty;

        public Dictionary<int, string> Queries { get; } = new Dictionary<int, string>();

        public HwAutomate(TextReader reader)
        {
            _reader = reader;
            _stateProcess = HwState.Start;
        }

        public void Process()
        {
            HwTask nums = null;
            var query = string.Empty;

            while (Next())
            {
                var lineState = _line.LineState();

                if (_stateProcess == HwState.Start && lineState != HwState.Begin)
                {
                    continue;
                }

                if (_stateProcess == HwState.Start && lineState == HwState.Begin ||
                    _stateProcess == HwState.End   && lineState == HwState.Begin)
                {
                    _stateProcess = lineState;
                    query = string.Empty;
                    nums = _line.ParseNumbers();
                    continue;
                }

                if (_stateProcess == HwState.Begin && lineState == HwState.Query ||
                    _stateProcess == HwState.Query && lineState == HwState.Query)
                {
                    _stateProcess = lineState;
                    query += _line;
                    continue;
                }

                if (_stateProcess == HwState.Query && lineState == HwState.End)
                {
                    _stateProcess = lineState;
                    if (nums != null)
                    {
                        Queries.Add(nums.Task, query);
                    }
                    continue;
                }
            }
        }

        private bool Next()
        {
            try
            {
                _line = _reader.ReadLine();
                if (_line == null)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public class HwChecker
    {
        public IList<Status> DoCheck(TextReader txt, int lessonId)
        {
            return null;
        }
    }

    public class Status
    {
        public bool Success { get; set; }
    }
}
