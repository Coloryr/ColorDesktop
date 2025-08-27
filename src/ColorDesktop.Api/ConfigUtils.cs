using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ColorDesktop.Api;

public static class ConfigUtils
{
    private static readonly object Locker = new();
    public static T Config<T>(T obj1, string path, JsonTypeInfo<T> type) where T : new()
    {
        var file = new FileInfo(path);
        T? obj;
        if (!file.Exists)
        {
            if (obj1 == null)
                obj = new T();
            else
                obj = obj1;
            Save(obj, path, type);
        }
        else
        {
            lock (Locker)
            {
                obj = JsonSerializer.Deserialize(File.ReadAllText(path), type) ?? obj1;
            }
        }
        return obj;
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    public static void Save<T>(T obj, string path, JsonTypeInfo<T> type)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(obj, type));
    }
}
