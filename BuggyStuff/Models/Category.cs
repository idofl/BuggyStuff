using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BuggyStuff.Models
{
    public class Category
    {
        public string Name { get; set; }
        private int[] _bigArray = new int[10000];

        ~Category()
        {
            Log();
        }

        static void Log()
        {
            // Write some log
            Thread.Sleep(100);
        }
    }
}
