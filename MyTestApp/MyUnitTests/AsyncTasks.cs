using System;
using System.Threading.Tasks;

namespace MyUnitTests
{

    static class AsyncTasks
    {
        public static async Task DisplayResultAsync()
        {
            int num = 5;
            var res = await FactorialAsync(num);
            await Task.Delay(5000);
            Console.WriteLine("Factolial of {0} is {1} [{2}]", num, res, DateTime.Now);
        }

        public static async Task<long> FactorialAsync(long i)
        {
            return await Task.Run(() => RecFact(i));
        }

        public static long RecFact(long i)
        {
            return i == 0 ? 1 : i*RecFact(i - 1);
        }

        public static async Task<int> FactorialForAsync(long i)
        {
            var res = 1;

            return await Task.Run(() =>
            {
                for (int j = 1; j <= i; j++)
                {
                    res *= j;
                }
                return res;
            });
        }
    }
}
