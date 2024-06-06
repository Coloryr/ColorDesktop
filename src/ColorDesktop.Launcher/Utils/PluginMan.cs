using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ColorDesktop.Api;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Utils;

public static class PluginMan
{
    public static readonly Dictionary<string, PluginDataObj> Plugins = [];
    public static readonly Dictionary<string, DllAssembly> PluginAssemblys = [];
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

            var ass = new DllAssembly(local, obj);
            PluginAssemblys.Add(obj.ID, ass);
        }
        catch(Exception e)
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
}

public class DllAssembly : AssemblyLoadContext
{
    private readonly PluginDataObj _obj;

    public IPlugin Plugin { get; init; }

    public string Local { get; init; }

    public DllAssembly(string local, PluginDataObj obj) : base(obj.ID, true)
    {
        Local = local;
        _obj = obj;

        foreach (var item in obj.Dlls)
        {
            var local1 = Path.GetFullPath(local + "/" + item + ".dll");
            var local2 = Path.GetFullPath(local + "/" + item + ".pdb");
            if (!File.Exists(local1))
            {
                continue;
            }

            using var stream = File.OpenRead(local1);
            if (File.Exists(local2))
            {
                using var stream1 = File.OpenRead(local2);

                LoadFromStream(stream, stream1);
            }
            else
            {
                LoadFromStream(stream);
            }
        }

        foreach (var item in Assemblies)
        {
            if (Plugin != null)
            {
                break;
            }
            foreach (var item1 in item.GetTypes())
            {
                if (Plugin != null)
                {
                    break;
                }
                foreach (var item2 in item1.GetInterfaces())
                {
                    if (item2 == typeof(IPlugin))
                    {
                        Plugin = (Activator.CreateInstance(item1) as IPlugin)!;
                        break;
                    }
                }
            }
        }

        if (Plugin == null)
        {
            throw new Exception("Plugin validation error");
        }
    }

    protected override Assembly? Load(AssemblyName name)
    {
        foreach (var item in _obj.Dependents)
        {
            if (PluginMan.PluginAssemblys.TryGetValue(item, out var ass))
            {
                var list = ass.Assemblies.Where(a => a.GetName().FullName == name.FullName);
                return list.First();
            }
        }

        return null;
    }
}