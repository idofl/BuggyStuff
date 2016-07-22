using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BuggyStuff
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Warn("Going to sleep for 4 seconds");
            Thread.Sleep(4000);
            Trace.Warn("Completed sleep");
        }
    }
}