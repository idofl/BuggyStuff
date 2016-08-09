using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ConcurrencyVisualizer.Instrumentation;

namespace BuggyStuff.Shared
{
    public static class Concurrency
    {
        static object lockObj = new object();
        public static void LockConvoy()
        {
            Task[] tasks = new Task[6];

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(
                    () =>
                    {
                        lock (lockObj)
                        {
                            Performance.HangFor(100);
                        }
                    }
                    );
            }
            Task.WaitAll(tasks);
        }

        public static void UnevenWorkload()
        {
            MarkerWriter myMarkerWriter = new MarkerWriter(new Guid("{915C680C-EA45-44F2-9921-FE3F531B4B47}"));

            Task[] tasks = new Task[4];
            int iterations = 100;
            int itemsPerTask = iterations / tasks.Length;
            for (int i = 0; i < tasks.Length; i++)
            {

                int start = itemsPerTask * i;
                MarkerSeries hangSeries = myMarkerWriter.CreateMarkerSeries("hang");

                tasks[i] = Task.Run(
                        () =>
                        {
                            for (int j = start; j < start + itemsPerTask; j++)
                            {
                                hangSeries.WriteFlag($"hanging for {j}ms");
                                Performance.HangFor(j);
                            }
                        }
                        );
            }
            Task.WaitAll(tasks);

            // Another option is to use Parallel.For, but it seems it uses work stealing
            // and therefore its hard to notice the uneven workload

            //List<int> numbers = Enumerable.Range(0, 100).ToList();
            //Parallel.For(0, numbers.Count,
            //    new ParallelOptions { MaxDegreeOfParallelism = 4},
            //    (item) =>
            //    {
            //        hangSeries.WriteFlag($"hanging for {item}ms");
            //        Performance.HangFor(item);                    
            //    });

        }

        public static void Oversubscription()
        {
            CountdownEvent counter = new CountdownEvent(100);
            IEnumerable<int> numbers = Enumerable.Range(0, 100);
            Parallel.ForEach(numbers,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                (item) =>
                {
                    Task.Run(() =>
                        {
                            Performance.HangFor(item);
                            counter.Signal();
                        }
                    );
                });
            counter.Wait();
        }
    }
}
