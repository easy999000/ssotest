
/// <summary>
/// 模拟数据
/// </summary>
public class AnalogData
{
    protected static Dictionary<string, AnalogData> tableList = new Dictionary<string, AnalogData>();

    public static AnalogData GetAnalogData(string name)
    {
        if (!tableList.ContainsKey(name))
        {
            tableList.Add(name, new AnalogData());
        }

        return tableList[name];
    }

    protected Dictionary<string, string> DicData { get; set; }
    = new Dictionary<string, string>();

    public void SetData(string key, string value)
    {
        if (DicData.ContainsKey(key))
        {
            DicData[key] = value;
        }
        else
        {
            DicData.Add(key, value);
        }

    }
    public string GetData(string key)
    {
        if (DicData.ContainsKey(key))
        {
            return DicData[key];
        }
        else
        {
            return null;
        }

    }

}

