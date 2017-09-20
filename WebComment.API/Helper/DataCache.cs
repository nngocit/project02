using System;
using System.Web;
using System.Web.Caching;

namespace VTA.Service.CoreCache
{
    public static class DataCache
    {
        private static Cache _objCache;
        public static readonly TimeSpan TimeExpired = new TimeSpan(0, 5, 0);

        // public const int TabCacheTimeOut = 20;
        //public const int TabModuleCacheTimeOut = 20;

        private static Cache ObjCache
        {
            get { return _objCache ?? (_objCache = HttpRuntime.Cache); }
        }

        public static void RemoveCache(string cacheKey)
        {
            ObjCache.Remove(cacheKey);
        }

        public static T GetCache<T>(string cacheKey) where T : class
        {
            T rs;
            try
            {
                rs = ObjCache.Get(cacheKey) as T;
            }
            catch
            {
                rs = null;
            }

            return rs;
        }

        public static object GetCache(string cacheKey)
        {
            return ObjCache.Get(cacheKey);

        }

        public static void SetCache(string keyName, object obj)
        {
            ObjCache.Insert(keyName, obj);
        }

        public static void SetCache(string keyName, object obj, CacheDependency dependency)
        {
            ObjCache.Insert(keyName, obj, dependency);
        }

        public static void SetCache(string keyName, object obj, CacheDependency dependency, DateTime expireTime, TimeSpan expireTimeSpan)
        {
            ObjCache.Insert(keyName, obj, dependency, expireTime, expireTimeSpan);
        }

        public static void SetCache(string keyName, object obj, TimeSpan expireTimeSpan)
        {
            ObjCache.Insert(keyName, obj, null, Cache.NoAbsoluteExpiration, expireTimeSpan);
        }

        public static void SetCache(string keyName, object obj, CacheDependency dependency, DateTime expireTime, TimeSpan expireTimeSpan, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            ObjCache.Insert(keyName, obj, dependency, expireTime, expireTimeSpan, priority, onRemoveCallback);
        }

        public static void SetCache(string keyName, object obj, DateTime expireTime)
        {
            ObjCache.Insert(keyName, obj, null, expireTime, Cache.NoSlidingExpiration);
        }

        #region Extension
        public static string GetCacheName(Type targetType, string subFixCache = null)
        {
            return string.Format("{0}.{1}", targetType, subFixCache);
        }

        //public static T FastCreateCache<T>(Type targetType, string cacheKey = null) where T : class, new()
        //{
        //    var keyCache = GetCacheName(targetType, cacheKey);

        //    var rs = GetCache<T>(keyCache);
        //    if (Equals(rs, null))
        //    {
        //        rs = new T();
        //        SetCache(keyCache, rs, DateTime.Now.Add(TimeExpired));
        //    }

        //    return rs;
        //}
        #endregion
    }
}
