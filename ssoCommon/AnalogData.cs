
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

    protected Dictionary<string, string> DicData { get; set; }
    = new Dictionary<string, string>();

    public void SetData<T>(string key, T value) where T : class
    {
        if (DicData.ContainsKey(key))
        {
            DicData[key] = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
        else
        {
            DicData.Add(key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
        }

    }
    public T GetData<T>(string key) where T : class
    {
        if (key == null)
        {
            return null;
        }
        if (DicData.ContainsKey(key))
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(DicData[key]);
        }
        else
        {
            return null;
        }

    }
    public void DelData(string key)
    {
        if (DicData.ContainsKey(key))
        {
            DicData.Remove(key);
        }


    }

}

