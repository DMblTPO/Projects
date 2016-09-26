using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskFactory
{
    public static class RbpConfig
    {
        static RbpConfig()
        {
            // Period property is Only! for debug and staging
            Period = new TimeSpan(0, 0, 20);
            // StartAt property is for Production
            IsProduction = false;
            StartAt = DateTime.UtcNow.TimeOfDay.Add(new TimeSpan(0, 0, 27));
        }

        public static TimeSpan Period { get; private set; }
        public static bool IsProduction { get; private set; }
        public static TimeSpan StartAt { get; private set; }
    }


    class Process
    {
        private CancellationTokenSource _cancellationSource;
        private bool _isFirstStart;

        private readonly object _lockObject = new object();
        private readonly object _lockObjectDbg = new object();

        public void Start()
        {
            lock (_lockObject)
            {
                _cancellationSource = new CancellationTokenSource();
                _isFirstStart = true;

                Task.Factory.StartNew(
                    ScanActiveContracts,
                    _cancellationSource.Token);
                Console.WriteLine("Task started!");
            }
        }

        private void ScanActiveContracts()
        {
            if (_cancellationSource != null && !_cancellationSource.IsCancellationRequested)
            {
                TimeSpan timeToStart;

                if (RbpConfig.IsProduction)
                {
                    var startAt = RbpConfig.StartAt;
                    var utcNow = DateTime.UtcNow;

                    var nextDateTime = utcNow.Date.Add(startAt);
                    if (startAt < utcNow.TimeOfDay)
                    {
                        nextDateTime = nextDateTime.AddDays(1);
                    }

                    timeToStart = nextDateTime - utcNow;
                }
                else
                {
                    lock (_lockObjectDbg)
                    {
                        if (_isFirstStart)
                        {
                            timeToStart = new TimeSpan(0, 0, 5);
                            _isFirstStart = false;
                        }
                        else
                        {
                            timeToStart = RbpConfig.Period;
                        }
                    }
                }

                Console.WriteLine("Waiting for {0}", timeToStart);

                _cancellationSource.Token.WaitHandle.WaitOne(timeToStart);

                Console.WriteLine("Scanning for active contracts to be invoiced is started...");

                Task.Run(() => GetAllActiveContracts()).ContinueWith(ProcessActiveContracts);
            }
        }

        private void GetAllActiveContracts()
        {
            int i = 0;
            for (int j = 0; j < 100000; j++)
            {
                i += j;
            }
            Console.WriteLine("Calc finish at {0}", DateTime.Now);
        }

        private void ProcessActiveContracts(Task obj)
        {
            Console.WriteLine("... post-process");
            ScanActiveContracts();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process();
            process.Start();
            Console.ReadKey();
        }
    }
}