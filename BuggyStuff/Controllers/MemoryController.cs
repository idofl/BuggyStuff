using BuggyStuff.Models;
using BuggyStuff.Shared;
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
        #region private fields
        private int[] _bigArray = new int[250000]; // 1MB array
        #endregion

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
            var product = Memory.CreateNewRegularProduct();
            Memory.CacheProduct(product, OnCacheItemRemovedMemoryCache);                    
        }

        [ActionName("GC")]
        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [ActionName("Gen0")]
        public void Gen0GC()
        {
            // Cause a lot of memory to be allocated, causing gen0 collection
            for (int i = 0; i < 1000; i++)
            {
                BigProduct product = Memory.CreateNewProduct();
            }
        }
        public void OnCacheItemRemovedMemoryCache(string key)
        {
            Trace.TraceInformation("Key has been evicted: " + key);
        }

        public void OnCacheItemRemovedHttpCache(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                Trace.TraceInformation("Key has been evicted: " + key);
            }
        }
    }
}
