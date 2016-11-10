using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuggyStuff.Shared
{
    public static class Hang
    {
        public static SomeType CreateObj(bool set)
        {
            return new SomeType(set);
        }
    }

    public class SomeType
    {
        ManualResetEvent mre = new ManualResetEvent(false);

        public SomeType(bool set)
        {
            if (set)
                mre.Set();
        }
        ~SomeType()
        {
            // mre never set, this will wait forever
            mre.WaitOne();
        }
    }
}
