using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuggyStuff.Shared
{
    public static class Performance
    {
        public static void KillCpu(int cpuUsage = 85)
        {                
            int time = 10000;
            List<Task> threads = new List<Task>();
            List<CancellationTokenSource> tokens = new List<CancellationTokenSource>();

            for (var i = 0; i < Environment.ProcessorCount; i++)
            {
                var cancelationTokenSource = new CancellationTokenSource();
                tokens.Add(cancelationTokenSource);
                threads.Add(Task.Factory.StartNew(
                    () => CPUKill(cpuUsage, cancelationTokenSource.Token),
                    tokens[i].Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default));
            }

            Thread.Sleep(time);

            for (var i = 0; i < Environment.ProcessorCount; i++)
            {
                tokens[i].Cancel();
            }
        }

        public static void CPUKill(object cpuUsage, CancellationToken ct)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;                    
                }

                if (watch.ElapsedMilliseconds > (int)cpuUsage)
                {
                    Thread.Sleep(100 - (int)cpuUsage);
                    watch.Reset();
                    watch.Start();
                }
            }
        }

        public static void HangFor(int duration)
        {
            Random rand = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > duration)
                {
                    break;
                }
                rand.Next(1000);
            }
        }
    }
}
