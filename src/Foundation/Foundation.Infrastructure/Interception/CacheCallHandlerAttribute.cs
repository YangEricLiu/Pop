using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using SE.DSP.Foundation.Infrastructure.Enumerations;
using System;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class CacheCallHandlerAttribute : HandlerAttribute
    {
        public string CacheName { get; set; }
        public int CacheKeyIndex { get; set; }
        public CacheOperation CacheOperation { get; set; }

        public CacheCallHandlerAttribute()
        {

        }

        public CacheCallHandlerAttribute(string cacheName, int cacheKeyIndex, CacheOperation cacheOperation)
        {
            this.CacheName = cacheName;
            this.CacheKeyIndex = cacheKeyIndex;
            this.CacheOperation = cacheOperation;
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new CacheCallHandler();
        }
    }
}