    using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BuggyStuff.Shared
{
    public static class Memory
    {
        static ObjectCache cache = MemoryCache.Default;
        static Random rand = new Random();

        public static BigProduct CreateNewProduct()
        {
            BigProduct product = new BigProduct
            {
                Name = Guid.NewGuid().ToString(),
                Photo = new Byte[rand.Next(60000, 80000)]
            };

            return product;
        }

        public static SmallProduct CreateNewRegularProduct()
        {
            SmallProduct product = new SmallProduct
            {
                Name = Guid.NewGuid().ToString(),
            };

            return product;
        }

        public static void CacheProduct(SmallProduct product, Action<string> removedCallback)
        {
            
            CacheItemPolicy policy = new CacheItemPolicy
            {
                RemovedCallback = ( args) =>
                {
                    removedCallback(args.CacheItem.Key);
                }
            };
            cache.Add(
                product.Id.ToString(), 
                product, 
                policy);
        }

        public static void CauseGC1And2()
        {
            // This will cause GC on gen1
            for (int i = 0; i < 1000; i++)
            {
                BigProduct product = Memory.CreateNewProduct();
            }

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                // This will cause GC on gen2
                GC.Collect();
            });
        }

        public static void AllocateMany()
        {
            List<BigProduct> bigProducts = new List<Shared.BigProduct>();
            List<SmallProduct> smallProducts = new List<Shared.SmallProduct>();

            for (int i=0; i<1000; i++)
            {
                bigProducts.Add(CreateNewProduct());
            }

            for (int i=0; i<1000000; i++)
            {
                smallProducts.Add(
                    new SmallProduct
                    {
                        Id = Guid.NewGuid(),
                        Name = "test"
                    });
            }
        }
    }

    public class SmallProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }

    public class BigProduct : SmallProduct
    {               
        public byte[] Photo { get; set; }
    }    
}
