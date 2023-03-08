using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 扩展类
/// </summary>
public static class Extension
{
    #region 扩展缓存


    public static void Set<T>(this IDistributedCache cache, string key, T Data, DistributedCacheEntryOptions option = null) where T : class
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
        cache.SetString(key, json, option);
    }
    public static T Get<T>(this IDistributedCache cache, string key) where T : class
    {

        var json = cache.GetString(key);
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return res;
        }
        catch (Exception)
        {
            return null;
        }


    }
    public static bool Set<T>(this IDatabase redis,
        RedisKey key, T value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None
         ) where T : class
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        return redis.StringSet(key, json, expiry, keepTtl, when, flags);
    }
    public static T Get<T>(this IDatabase redis, string key) where T : class
    {

        string json = redis.StringGet(key);
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return res;
        }
        catch (Exception)
        {
            return null;
        }

    }
    #endregion
}

