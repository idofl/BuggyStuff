using BuggyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BuggyStuff.Controllers
{
    public class CrashesController : ApiController
    {
        PagedContent _data;

        public CrashesController()
        {
            _data = new PagedContent();
            _data.Content = new List<string> { "this", "is", "a", "test" };
        }

        [Route("api/crash/stop")]
        [HttpGet]
        public void StopWorkerProcess()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
                {
                    Thread.Sleep(100);
                    throw new Exception("Crashed");
                }
                ));

            Thread.Sleep(3000);
        }

        public IEnumerable<string> GetDataPage(int index)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            IEnumerable<string> result = _data.GetPage(index, 2);

            // Check if we reached the end of the data
            if (result.Count() == 0)
                _data.Content = null;

            return result;
        }

        [Route("api/crashes/oops")]
        [HttpGet]
        public void CrashMe()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                Thread.Sleep(1000);
                throw new Exception("oops");
            }));
        }
    }
}
