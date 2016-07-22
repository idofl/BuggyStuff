using BuggyStuff.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace BuggyStuff.Controllers
{
    public class MemoryController : Controller
    {
        private int[] _bigArray = new int[10000];

        [ActionName("dictionary")]
        public JsonResult LargeDictionaryAllocations()
        {

            Dictionary<int, Category> categories = new Dictionary<int, Category>();

            for (int i = 0; i < 1000; i++)
            {
                categories.Add(i, new Category { Name = "Category " + i.ToString() });
            }

            return Json(categories.Values, JsonRequestBehavior.AllowGet);
        }

        /*
        [ActionName("serializer")]
        public string GetProductAsXmlString()
        {
            ProductEx product = new ProductEx
            {
                Id = 1,
                Name = "Product1",
                MainCategory = new Category
                {
                    Name = "CategoryA"
                }
            };

            Type[] extraTypes = new Type[1] { typeof(Category) };

            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(typeof(ProductEx), extraTypes);
            xs.Serialize(ms, product);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            string result = Encoding.Default.GetString(ms.GetBuffer());
            ms.Close();

            return result;
        }
        */

        [ActionName("Cache")]
        public void CacheProduct()
        {
            ProductsRepository repository = new ProductsRepository();

            var product = repository.GetProduct<Product>(0, "Product");

            this.HttpContext.Cache.Add(
                Guid.NewGuid().ToString(),
                product,
                null,
                DateTime.MaxValue,
                TimeSpan.FromHours(1),
                CacheItemPriority.Normal,
                OnCacheItemRemoved
                );
        }

        [ActionName("GC")]
        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                Trace.TraceInformation("Key has been evicted: " + key);
            }
        }
    }
}
