using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuggyStuff.Models
{
    public class PagedContent
    {
        public IEnumerable<string> Content { get; set; }

        public IEnumerable<string> GetAll()
        {
            return Content;
        }

        public IEnumerable<string> GetPage(int page, int size)
        {
            return Content.Skip((page - 1) * size).Take(size);
        }

        ~PagedContent()
        {
            if (Content.Count() > 0)
            {
                Content = null;
            }
        }
    }
}
