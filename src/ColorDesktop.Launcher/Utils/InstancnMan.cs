using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorDesktop.Api;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Utils;

public static class InstancnMan
{
    public static readonly List<string> KnowUUID = [];
    public static readonly Dictionary<string, InstanceDataObj> Instancns = [];

    /// <summary>
    /// 读取时失败
    /// </summary>
    public static readonly Dictionary<string, Exception> LoadError = [];
    /// <summary>
    /// 加载时失败
    /// </summary>
    public static readonly Dictionary<string, Exception> LoadFail = [];

    public const string Dir2 = "instances";
    public const string FileName = "instance.json";
    private static string local2;
    public static void Init()
    {
        var local = AppContext.BaseDirectory;
        local2 = Path.GetFullPath(local + Dir2);
        Directory.CreateDirectory(local2);
        var info = new DirectoryInfo(local2);
        foreach (var item in info.GetDirectories())
        {
            try
            {
                var uuid = item.Name;
                KnowUUID.Add(uuid);
                var config = item.GetFiles().FirstOrDefault(item => item.Name.Equals(FileName, StringComparison.CurrentCultureIgnoreCase));
                if (config != null)
                {
                    var obj = JsonConvert.DeserializeObject<InstanceDataObj>(File.ReadAllText(config.FullName));
                    if (obj == null)
                    {
                        continue;
                    }
                    if (obj.UUID != uuid)
                    {
                        obj.UUID = uuid;
                        ConfigUtils.Save(obj, config.FullName);
                    }
                    Instancns.Add(uuid, obj);
                }
            }
            catch (Exception e)
            {
                LoadError.Add(item.Name, e);
            }
        }

        foreach (var item in Instancns)
        {
            if (!PluginMan.PluginAssemblys.ContainsKey(item.Value.Plugin))
            {
                LoadFail.Add(item.Key, new Exception("not found plugin " + item.Value.Plugin));
            }
        }
    }

    public static string Create(InstanceDataObj obj)
    {
        string dir = Path.GetFullPath(local2 + "/" + obj.UUID);
        Directory.CreateDirectory(dir);
        ConfigUtils.Save(obj, Path.GetFullPath(dir + "/" + FileName));

        return dir;
    }
}
