using System;

namespace MyUnitTests
{
    public class ScheduleTime
    {
        public class NextTs
        {
            public TimeSpan Time;
            public TimeSpan Remain;
        }

        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _period;
        private readonly TimeSpan _endOfDay;
        private TimeSpan _lastNext;

        public ScheduleTime(TimeSpan dueTime, TimeSpan period)
        {
            _dueTime = dueTime;
            _period = period;
            _endOfDay = new TimeSpan(24, 0, 0);
            _lastNext = dueTime;
        }

        public NextTs NextRun(bool rightNow = false)
        {
            TimeSpan curTime = DateTime.UtcNow.TimeOfDay;

            if (rightNow)
            {
                return new NextTs {Time = curTime, Remain = new TimeSpan(0, 0, 5)};
            }

            while (_lastNext < curTime)
            {
                _lastNext += _period;
            }


            if (_lastNext > _endOfDay)
            {
                return new NextTs {Time = _dueTime, Remain = _dueTime + _endOfDay - curTime};
            }

            return new NextTs {Time = _lastNext, Remain = _lastNext - curTime};
        }
    }
}