using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuggyStuff.Models
{
    public class ProductsRepository
    {
        public IEnumerable<Product> GetShortListWithLongNames()
        {
            return GenerateProductList<Product>(100, "Product".PadLeft(5000, '-').PadRight(5000, '-'));
        }

        public IEnumerable<Product> GetShortList()
        {
            return GenerateProductList<Product>(100);
        }

        public IEnumerable<Product> GetLongList()
        {
            return GenerateProductList<Product>(100000);
        }

        public IEnumerable<Product> GetLongExList()
        {
            return GenerateProductList<ProductEx>(100000);
        }

        private IEnumerable<Product> GenerateProductList<T>(int size) where T: Product, new()
        {
            return GenerateProductList<T>(size, "Product");
        }
        private IEnumerable<Product> GenerateProductList<T>(int size, string prefix) where T : Product, new()
        {
            var query = from i in Enumerable.Range(0, size)
                        select GetProduct<T>(i, prefix);

            return query;
        }

        public Product GetProduct<T>(int index, string prefix) where T : Product, new()
        {
            return new T
             {
                 Id = index,
                 Name = prefix + index
             };
        }
    }
}
