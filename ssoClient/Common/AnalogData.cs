
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
/// <summary>
/// 模拟数据,演示程序,替代数据库
/// </summary>
public class AnalogData
{
    protected static Dictionary<string, AnalogData> tableList = new Dictionary<string, AnalogData>();


    public static AnalogData GetAnalogData(Enum name)
    {
        if (!tableList.ContainsKey(name.ToString()))
        {
            tableList.Add(name.ToString(), new AnalogData());
        }

        return tableList[name.ToString()];
    }

    protected MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
    protected Dictionary<string, string> DicData { get; set; }
    = new Dictionary<string, string>();

    /// <summary>
    /// 设置缓存 默认2小时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetData<T>(string key, T value) where T : new()
    {
        Cache.Set(key, value, TimeSpan.FromHours(2));
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T GetData<T>(string key) where T : new()
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return default;
        }
        return Cache.Get<T>(key);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public void DelData(string key)
    {
        Cache.Remove(key);

    }

    /// <summary>
    /// 续期
    /// </summary>
    public void Renewal(string key)
    {
        var o = Cache.Get(key);

        Cache.Set(key, o, TimeSpan.FromHours(2));


    }
}

