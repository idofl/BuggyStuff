using BuggyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BuggyStuff.Controllers
{
    public class ExceptionsController : Controller
    {
        ProductsRepository _repository;

        public ExceptionsController()
        {
            _repository = new ProductsRepository();
        }
        public JsonResult GetShortList()
        {
            return Json(_repository.GetShortList(), JsonRequestBehavior.AllowGet);
        }
       

        public JsonResult GetLongList()
        {
            JsonResult result = Json(_repository.GetLongList(), JsonRequestBehavior.AllowGet);
            //result.MaxJsonLength = int.MaxValue;
            return result;
        }

        [Authorize]
        public JsonResult GetSecuredList()
        {
            return GetShortList();
        }

    }
}
