using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorDesktop.Api;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Utils;

public static class InstanceMan
{
    public static readonly List<string> KnowUUID = [];
    public static readonly Dictionary<string, InstanceDataObj> Instances = [];

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

    private static string s_workDir;

    public static void Init()
    {
        s_workDir = Path.GetFullPath(AppContext.BaseDirectory + Dir2);
        Directory.CreateDirectory(s_workDir);
        var info = new DirectoryInfo(s_workDir);
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
                    Instances.Add(uuid, obj);
                }
            }
            catch (Exception e)
            {
                LoadError.Add(item.Name, e);
            }
        }

        foreach (var item in Instances)
        {
            if (!PluginMan.PluginAssemblys.ContainsKey(item.Value.Plugin))
            {
                LoadFail.Add(item.Key, new Exception("not found plugin " + item.Value.Plugin));
            }
        }
    }

    public static void Create(InstanceDataObj obj)
    {
        var dir = GetLocal(obj);
        Directory.CreateDirectory(dir);
        ConfigUtils.Save(obj, Path.GetFullPath(dir + "/" + FileName));
        Instances.Add(obj.UUID, obj);
        KnowUUID.Add(obj.UUID);
        ConfigHelper.EnableInstance(obj.UUID);
    }

    public static string GetLocal(InstanceDataObj obj)
    {
        return Path.GetFullPath(s_workDir + "/" + obj.UUID);
    }

    public static string GetDataLocal(InstanceDataObj obj)
    {
        return Path.GetFullPath(s_workDir + "/" + obj.UUID + "/" + FileName);
    }
}
