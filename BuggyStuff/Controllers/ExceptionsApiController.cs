using BuggyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BuggyStuff.Controllers
{
    [Route("api/exceptions")]
    public class ExceptionsApiController : ApiController
    {
        ProductsRepository _repository;

        public ExceptionsApiController()
        {
            _repository = new ProductsRepository();
        }

        public IEnumerable<Product> GetLongList()
        {
            //System.Diagnostics.Trace.TraceError()
            var result = _repository.GetLongExList();
            return result;
        }
    }
}
