using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Manager;

public static class PluginManager
{
    public static readonly Dictionary<string, PluginDataObj> Plugins = [];
    public static readonly Dictionary<string, PluginAssembly> PluginAssemblys = [];
    public static readonly Dictionary<string, List<string>> Deps = [];

    /// <summary>
    /// 读取时失败
    /// </summary>
    public static readonly List<(string, Exception)> LoadError = [];
    /// <summary>
    /// 加载时失败
    /// </summary>
    public static readonly List<(string, Exception)> LoadFail = [];

    public const string Dir1 = "plugins";
    public static void Init()
    {
        var local = AppContext.BaseDirectory;
        var local1 = Path.GetFullPath(local + Dir1);
        Directory.CreateDirectory(local1);

        var list1 = Directory.GetDirectories(local1);
        foreach (var item in list1)
        {
            try
            {
                var list2 = PathHelper.GetAllFile(item);
                var config = list2.FirstOrDefault(item => item.Name.Equals("plugin.json", StringComparison.CurrentCultureIgnoreCase));
                if (config != null)
                {
                    var obj = JsonConvert.DeserializeObject<PluginDataObj>(File.ReadAllText(config.FullName));
                    if (obj == null)
                    {
                        continue;
                    }
                    LoadPlugin(config.DirectoryName!, obj);
                }
            }
            catch (Exception e)
            {
                LoadError.Add((item, e));
            }
        }

        foreach (var item in Plugins)
        {
            foreach (var item1 in item.Value.Dependents)
            {
                if (!PluginAssemblys.ContainsKey(item1))
                {
                    LoadFail.Add((item.Key, new Exception("Dependent not found")));
                    break;
                }
                if (Deps.TryGetValue(item1, out var list))
                {
                    list.Add(item.Key);
                }
                else
                {
                    Deps.Add(item1, [item.Key]);
                }
            }
        }

        foreach (var item in LoadFail)
        {
            if (PluginAssemblys.Remove(item.Item1, out var ass))
            {
                ass.Unload();
            }

            ConfigHelper.Config.EnablePlugin.Remove(item.Item1);
        }
    }

    public static void LoadPlugin(string local, PluginDataObj obj)
    {
        try
        {
            if (!Plugins.TryAdd(obj.ID, obj))
            {
                throw new Exception("Plugin Info Add Fail, ID:" + obj.ID);
            }

            var ass = new PluginAssembly(local, obj);
            PluginAssemblys.Add(obj.ID, ass);
        }
        catch (Exception e)
        {
            LoadFail.Add((obj.ID, e));
        }
    }

    public static bool IsFail(string id)
    {
        foreach (var item in LoadFail)
        {
            if (item.Item1 == id)
            {
                return true;
            }
        }

        return false;
    }

    public static void StartPlugin()
    {
        foreach (var item in ConfigHelper.Config.EnablePlugin)
        {
            if (PluginAssemblys.TryGetValue(item, out var ass))
            {
                ass.Plugin.Init(ass.Local, LanguageType.zh_cn);
            }
        }
    }

    public static void OpenSetting(PluginDataObj obj)
    {
        if (PluginAssemblys.TryGetValue(obj.ID, out var value))
        {
            value.Plugin.OpenSetting();
        }
    }

    public static (bool, string?) EnablePlugin(string id)
    {
        ConfigHelper.EnablePlugin(id);

        return (true, null);
    }

    public static (bool, string?) DisablePlugin(string id)
    {
        ConfigHelper.DisablePlugin(id);
        return (true, null);
    }

}
