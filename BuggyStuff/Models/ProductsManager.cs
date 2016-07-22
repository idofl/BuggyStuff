using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuggyStuff.Models
{
    public class ProductsManager
    {
        private static ProductsManager _instance = new ProductsManager();
        private object _syncobj = new object();
        private ProductsRepository _repository = new ProductsRepository();

        private ProductsManager()
        {

        }

        public static ProductsManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public Product GetProduct(int index)
        {
            lock (_syncobj)
            {
                Thread.Sleep(1000);
                return _repository.GetProduct<Product>(index, "Product");
            }
        }

        public IEnumerable<Product> GetManyProducts()
        {
            IEnumerable<Product> result = _repository.GetShortListWithLongNames();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return result;
        }
    }
}
