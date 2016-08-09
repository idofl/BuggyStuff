using BuggyStuff.Models;
using BuggyStuff.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BuggyStuff.Controllers
{
    public class PerformanceController : ApiController
    {
        public IEnumerable<Product> GetManyProducts()
        {
            return ProductsManager.Instance.GetManyProducts();
        }


        [Route("api/latency/expression")]
        [HttpGet]
        public string DoExpressions()
        {
            Stopwatch watch = new Stopwatch();
            long result = 0;
            watch.Start();
            for (int index = 0; index < 10000; index++)
            {
                result += DoExpression(i => i + 1, index);
            }
            watch.Stop();
            return "time taken : " + watch.ElapsedMilliseconds + ", results: " + result;
        }

        [Route("api/latency/lambda")]
        [HttpGet]
        public string DoLambdas()
        {
            Stopwatch watch = new Stopwatch();
            int result = 0;
            watch.Start();
            for (int index = 0; index < 10000; index++)
            {
                result += DoLambda(i => i + 1, index);
            }
            watch.Stop();
            return "time taken : " + watch.ElapsedMilliseconds + ", results: " + result;
        }

        private long DoExpression(Expression<Func<int, long>> expr, int value)
        {
            return expr.Compile()(value);
        }

        private int DoLambda(Func<int, int> lambda, int value)
        {
            return lambda(value);
        }

        [Route("api/profiling/prime")]
        public List<int> GetPrimes()
        {
            List<int> primes = new List<int>();
            for (int i = 0; i < 100000; ++i)
                if (PrimeUtil.IsPrime(i))
                {
                    primes.Add(i);
                };

            return primes;
        }

     
        [Route("api/cpu/{cpuUsage:int?}")]
        [HttpGet]
        public void GenerateLoad(int cpuUsage = 85)
        {
            Performance.KillCpu(cpuUsage);           
        }

        [Route("api/latency/linear/{delay}/{chunkSize}")]
        [HttpGet]
        public HttpResponseMessage GetDelayedBodyAsLinear(int delay, int chunkSize)
        {
            return GenerateResponse(delay, chunkSize, (idx, size) => size * (idx + 1));
        }

        [Route("api/latency/exponential/{delay}/{chunkSize}")]
        [HttpGet]
        public HttpResponseMessage GetDelayedBodyAsExponential(int delay, int chunkSize)
        {
            return GenerateResponse(delay, chunkSize, (idx, size) => (int)Math.Pow(size, idx));
        }

        private static HttpResponseMessage GenerateResponse(int delay, int chunkSize, Func<int, int, int> sizeDelegate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new PushStreamContent((responseStream, httpContent, context) =>
            {
                responseStream.Flush();
                // Nothing to write yet
                Thread.Sleep(3000);

                // Start writing with delays
                StreamWriter responseStreamWriter = new StreamWriter(responseStream);
                // Write one byte
                responseStreamWriter.WriteLine("1");
                responseStreamWriter.Flush();
                // Write the rest
                for (int i = 0; i < 10; i++)
                {
                    int size = sizeDelegate(i, chunkSize);
                    responseStreamWriter.WriteLine($"writing {size} bytes");
                    responseStreamWriter.WriteLine(string.Empty.PadLeft(size, '-'));
                    responseStreamWriter.Flush();
                    Thread.Sleep(delay * i);
                }
                responseStreamWriter.Close();
            }, "text/plain");

            return response;
        }      
    }

    public static class PrimeUtil
    {
        public static bool IsPrime(int number)
        {
            if (number == 0 || number == 1)
                return false;

            // Optimize this to call Math.Sqrt only once.

            for (int i = 2; i < (int)Math.Ceiling(Math.Sqrt(number)); ++i)
                if (number % i == 0) return false;
            return true;
        }
    }
}
