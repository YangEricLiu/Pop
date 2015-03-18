
using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;

namespace SE.DSP.Foundation.Web
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheAttribute : Attribute
    {
        public string CacheName { get; set; }
        public int CacheKeyIndex { get; set; }
        public CacheOperation CacheOperation { get; set; }

        public CacheAttribute(string cacheName, int cacheKeyIndex, CacheOperation cacheOperation)
        {
            this.CacheName = cacheName;
            this.CacheKeyIndex = cacheKeyIndex;
            this.CacheOperation = cacheOperation;
        }
    }
}