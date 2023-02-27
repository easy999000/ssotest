using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CatchHelper
    {
        private static IDistributedCache Cache;

        /// <summary>
        /// 静态初始化,缓存帮助类
        /// </summary>
        /// <param name="cache"></param>
        public static void InitCache(IDistributedCache cache)
        {

            Cache = cache;
        }


        public static T Get<T>(string key) where T : class
        {

            var res = Cache.Get<T>(key);

            return res;

        }

        public static void Set<T>(string key, T data, int expireSeconds = 600) where T : class
        { 
            Cache.Set(key, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSeconds)
            });
             
        }

        public static T GetOrSet<T>(string key, Func<T> fun, int expireSeconds = 600) where T : class
        { 
            var res = Cache.Get<T>(key);
            if (res == null)
            {
                lock (Cache)
                {
                    if (res == null)
                    {
                        var data = fun();

                        Cache.Set(key, data, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSeconds)
                        });
                        res = data;
                    }
                }
            }
            return res;

        }
    }
}
