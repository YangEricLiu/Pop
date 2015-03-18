/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: CacheHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for cache
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using Microsoft.ApplicationServer.Caching;
using Microsoft.Practices.Unity;
using System.Configuration;

using System;
using SE.DSP.Foundation.Infrastructure.Enumerations;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    /// <summary>
    /// The cache utility class for AppFabric
    /// </summary>
    /// <typeparam name="T">Entity type which must be serializable</typeparam>
   public class CacheHelper
    {
        private static bool isCacheEnable;

        private string cacheName;
        private DataCache dCache = null;
        static DataCacheFactory cacheFactory = null;
       /// <summary>
       /// Create REMCache instatnce,
       /// </summary>
        /// <param name="cache">CacheName</param>
       /// <returns></returns>
       public CacheHelper(string cache, long? spId)
        {
            cacheName = cache;
            dCache = null;
            IocHelper.Container.RegisterType<DataCacheFactory, DataCacheFactory>();
            isCacheEnable = bool.Parse(ConfigurationManager.AppSettings["CacheEnable"] ?? "false");
            bool isInCacheDomain = bool.Parse(ConfigurationManager.AppSettings[cacheName] ?? "false");
            if (!isCacheEnable)
            {
                return;
            }
            else if (!isInCacheDomain)
            {
                //LogHelper.LogError(cacheName+" is not in Cache domain");
                return;
            }
            else
            {
                try
                {
                    if (spId.HasValue)
                    {
                        cacheName = cache + "_" + spId.Value;
                    }
                    var factory = GetDataCacheFactory();

                    dCache = factory.GetCache(cacheName);
                    //LogHelper.LogInformation("get cache success");
                }
                catch (DataCacheException dce)
                {
                    LogHelper.LogError("get cache failed in Constructor+" + dce.Message);
                    dCache = null;
                    return;
                }
                catch (ResolutionFailedException rfe)
                {
                    LogHelper.LogError("get cache failed in Constructor because of resolution exception+" + rfe.Message);
                    dCache = null;
                    return;
                }
            }
        }

       private DataCacheFactory GetDataCacheFactory()
       {
           if (cacheFactory != null)
               return cacheFactory;

           var hostStr = ConfigHelper.Get(DeploymentConfigKey.AppFabricServerHosts);

           if (string.IsNullOrWhiteSpace(hostStr)) return null;

           var hosts = hostStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
           if (hosts.Length == 0) return null;

           var servers = new DataCacheServerEndpoint[hosts.Length];
           for (int i = 0; i < hosts.Length; i++)
           {
               servers[i] = new DataCacheServerEndpoint(hosts[i], 22233);
           }

           // Setup the DataCacheFactory configuration.
           DataCacheFactoryConfiguration factoryConfig = new DataCacheFactoryConfiguration();
           factoryConfig.Servers = servers;

           // Create a configured DataCacheFactory object.
           cacheFactory = new DataCacheFactory(factoryConfig);
           return cacheFactory;
       }


       /// <summary>
        /// Cache object in the cache with the specified key. 
       /// </summary>
       /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="key">The type of the object which need be cached.</param>
       /// <returns>if null returned, means not in Cache or Cache is disabled</returns>
        public TValue Get<TKey, TValue>(TKey key)
        {
            TValue value = default(TValue);
            if (dCache == null)
            {
                //LogHelper.LogInformation("GetObject: DataCache object is null");
                return default(TValue);
            }
            try{
            value = (TValue)dCache.Get(Convert.ToString(key));
             }
            catch (Exception cacheException)
            {
                LogHelper.LogError("GetObject failed,key="+Convert.ToString(key)+" ,exception message is: " + cacheException.Message);
            }
            return value;
        }

        /// <summary>
        /// Cache object in the cache with the specified key, If object is not in cache, 
        /// Callback function need to return the object and FW will save it to Cache
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="cache">object get from CreateREMCache method</param>
        /// <param name="key">The type of the object which need be cached.</param>
        /// <param name="getObject">This function need to return the object and FW will set to cache</param>
        /// <returns>if null returned, means not in Cache or Cache is disabled</returns>
        public TValue Get<TKey, TValue>(TKey key, Func<TValue> getObject)
        {
            TValue value = default(TValue);
            if (dCache == null)
            {
               // LogHelper.LogInformation("GetObject: DataCache object is null");
                return getObject();
            }
            string strKey = Convert.ToString(key);
            try
            {
                value = (TValue)dCache.Get(strKey);
            }
            catch (System.Exception cacheException)
            {
                LogHelper.LogError("GetObject failed, exception message is: " + cacheException.Message);
            }
            if (value == null)
            {
                LogHelper.LogInformation(cacheName + strKey + "is not in cache");
                value = getObject();
                if (value != null)
                {
                    Set(strKey, value);
                }
            }
            return value;
        }


       /// <summary>
       ///  remove the object from CacheServer
       /// </summary>
        /// <param name="cache">object get from CreateREMCache method</param>
       /// <param name="key">the key for Cache</param>
       /// <returns></returns>
        public bool Remove<TKey>(TKey key)
        {
            bool rst;
            if (dCache == null)
            {
               // LogHelper.LogInformation("removeObject: DataCache object is null");
                return false;
            }

            rst = dCache.Remove(key.ToString());
            return rst;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="TKey"></typeparam>
        /// <param name="cache">object get from CreateREMCache method</param>
       /// <param name="key"></param>
       /// <param name="value"></param>
       /// <returns></returns>
        public bool Set<TKey, TValue>(TKey key, TValue value)
        {
            if (dCache == null)
            {
               // LogHelper.LogInformation("AddObject: DataCache object is null");
                return false;
            }

            if (value != null)
            {
                dCache.Put(key.ToString(), value);
            }
            return true;
        }
    }
}
