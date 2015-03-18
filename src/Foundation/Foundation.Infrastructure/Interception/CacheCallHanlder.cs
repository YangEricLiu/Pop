using Microsoft.Practices.Unity.InterceptionExtension;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Utils;
using System;
using System.Configuration;

namespace SE.DSP.Foundation.Infrastructure.Interception
{
    public class CacheCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn msg = null;

            #region Cache Setting
            CacheCallHandlerAttribute cacheAttribute = null;
            CacheCallHandlerAttribute[] cacheAttributes = (CacheCallHandlerAttribute[])input.MethodBase.GetCustomAttributes(typeof(CacheCallHandlerAttribute), true);

            bool cacheEnabled = false;

            if (cacheAttributes.Length > 0)
            {
                cacheAttribute = cacheAttributes[0];

                var cacheEnabledSetting = ConfigurationManager.AppSettings[cacheAttribute.CacheName];

                cacheEnabled = string.IsNullOrEmpty(cacheEnabledSetting) ? false : Convert.ToBoolean(cacheEnabledSetting);
            }
            #endregion

            #region process cache
            if (cacheEnabled)
            {
                var cacheName = cacheAttribute.CacheName;
                object cacheKeyObject = input.Inputs[cacheAttribute.CacheKeyIndex];
                object cacheKey;

                var cache = new CacheHelper(cacheName, ServiceContext.CurrentUser.SPId);

                switch (cacheAttribute.CacheOperation)
                {
                    #region Create
                    case CacheOperation.Create:
                        msg = getNext()(input, getNext);

                        cacheKey = this.GetCacheKey(msg.ReturnValue); //not use cacheKeyObject because generate after create

                        //save to cache
                        cache.Set(cacheKey, msg.ReturnValue);

                        break;
                    #endregion

                    #region Retrieve
                    case CacheOperation.Retrieve:
                        cacheKey = this.GetCacheKey(cacheKeyObject);

                        var cacheValue = cache.Get<object, object>(cacheKey);

                        if (cacheValue == null) //not in cache
                        {
                            msg = getNext()(input, getNext);

                            //save into cache
                            if (msg.ReturnValue != null)
                            {
                                cache.Set(cacheKey, msg.ReturnValue);
                            }
                        }
                        else //return the object in the cache
                        {
                            msg = input.CreateMethodReturn(cacheValue);
                        }

                        break;
                    #endregion

                    #region Update
                    case CacheOperation.Update:
                        cacheKey = this.GetCacheKey(cacheKeyObject);

                        msg = getNext()(input, getNext);

                        if (msg.Outputs[0] == null)
                        {
                            //delete from cache
                            cache.Remove(cacheKey);
                        }
                        else
                        {
                            //update cache
                            cache.Set(cacheKey, msg.ReturnValue);
                        }

                        break;
                    #endregion

                    #region Delete
                    case CacheOperation.Delete:
                        cacheKey = this.GetCacheKey(cacheKeyObject);

                        msg = getNext()(input, getNext);

                        //delete from cache
                        cache.Remove(cacheKey);

                        break;
                    #endregion
                }
            }
            #endregion

            #region default
            else
            {
                msg = getNext()(input, getNext);
            }
            #endregion

            return msg;
        }

        private object GetCacheKey(object cacheKeyObject)
        {
            object cacheKey;

            if (cacheKeyObject is ValueType)
            {
                cacheKey = cacheKeyObject;
            }
            else
            {
                var getCacheKeyMethod = cacheKeyObject.GetType().GetMethod("GetCacheKey");

                cacheKey = getCacheKeyMethod.Invoke(cacheKeyObject, null);
            }

            return cacheKey;
        }
    }
}