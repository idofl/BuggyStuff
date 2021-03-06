﻿using BuggyStuff.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BuggyStuff.Controllers
{
    public class HangController : ApiController
    {
        private static object syncobjA = new object();
        private static object syncobjB = new object();     

        [Route("api/hang/lock/{id}")]
        [HttpGet]
        public Product GetProduct(int id)
        {
            return ProductsManager.Instance.GetProduct(id);
        }

        [Route("api/hang/a")]
        [HttpGet]
        public string HangA()
        {
            lock (syncobjA)
            {
                Thread.Sleep(10000);
                lock (syncobjB)
                {
                    return "A done";
                }
            }
        }

        [Route("api/hang/b")]
        [HttpGet]
        public string HangB()
        {
            lock (syncobjB)
            {
                Thread.Sleep(10000);
                lock (syncobjA)
                {
                    return "B done";
                }
            }
        }

        [Route("api/hang/forever")]
        [HttpGet]
        public async Task<string> HangForever()
        {
            // Hang, but don't incur CPU
            while(true)
            {
                await Task.Delay(1000);
            }
        }

        [Route("api/hang/finalizer")]
        [HttpGet]
        public string Finalizer()
        {
            var obj = BuggyStuff.Shared.Hang.CreateObj(false);
            var task = Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                });
            obj = null;
            return "Finalizer stopped working. Try allocating lots of memory (/memory/dictionary), or unloading the appDomain";
        }      
    }   
}
