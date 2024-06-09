using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorDesktop.Api;

public static class ConfigUtils
{
    private static readonly object Locker = new();
    public static T Config<T>(T obj1, string FilePath) where T : new()
    {
        var file = new FileInfo(FilePath);
        T? obj;
        if (!file.Exists)
        {
            if (obj1 == null)
                obj = new T();
            else
                obj = obj1;
            Save(obj, FilePath);
        }
        else
        {
            lock (Locker)
            {
                obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath)) ?? obj1;
            }
        }
        return obj;
    }
    /// <summary>
    /// 保存配置文件
    /// </summary>
    public static void Save(object obj, string FilePath)
    {
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
    }
}
