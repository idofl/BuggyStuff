using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BuggyStuff.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [ActionName("Session")]
        public ActionResult WithSession()
        {
            Session["something"] = "something";
            Thread.Sleep(1000);
            ViewBag.Title = "Home Page";

            return View("Index");
        }
    }
}
