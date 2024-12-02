using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using DialogHostAvalonia;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Manager;

public static class PluginManager
{
    public static readonly Dictionary<string, PluginDataObj> Plugins = [];
    public static readonly Dictionary<string, PluginAssembly> PluginAssemblys = [];
    public static readonly Dictionary<string, PluginState> PluginStates = [];
    public static readonly Dictionary<string, string> PluginDir = [];
    public static readonly Dictionary<string, Dictionary<string, bool>> Controls = [];

    public const string Dir1 = "plugins";
    public const string ConfigName = "plugin.json";

    public static string RunDir { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        RunDir = Path.GetFullPath(Program.RunDir + Dir1);
        Directory.CreateDirectory(RunDir);

        var list = new HashSet<string>();

        foreach (var item in Directory.GetDirectories(RunDir))
        {
            try
            {
                var list2 = PathHelper.GetAllFile(item);
                var config = list2.FirstOrDefault(item =>
                    item.Name.Equals(ConfigName, StringComparison.CurrentCultureIgnoreCase));
                if (config == null)
                {
                    continue;
                }
                var obj = JsonConvert.DeserializeObject<PluginDataObj>(File.ReadAllText(config.FullName));
                if (obj == null)
                {
                    continue;
                }

                if (Plugins.ContainsKey(obj.ID))
                {
                    Logs.Error(string.Format("组件 {0} 存在重复的ID", obj.ID));
                    continue;
                }

                Plugins.Add(obj.ID, obj);
                PluginDir.Add(obj.ID, config.DirectoryName!);

                if (obj.ApiVersion != Program.ApiVersion)
                {
                    Logs.Error(string.Format("组件 {0} 的API版本不一致", obj.ID));
                    list.Add(obj.ID);
                    SetPluginState(obj.ID, PluginState.LoadError);
                    continue;
                }
                if (!CheckOs(obj.Os))
                {
                    Logs.Error(string.Format("组件 {0} 的系统支持列表不支持该系统", obj.ID));
                    list.Add(obj.ID);
                    SetPluginState(obj.ID, PluginState.OsError);
                    continue;
                }

                PluginAssemblys.Add(obj.ID, new PluginAssembly(config.DirectoryName!, obj));
            }
            catch (Exception e)
            {
                Logs.Error(string.Format("组件 {0} 加载失败", item), e);
            }
        }

        foreach (var item in PluginAssemblys)
        {
            foreach (var item1 in item.Value.Obj.Dependents)
            {
                if (!PluginAssemblys.TryGetValue(item1.ID, out var ass1))
                {
                    list.Add(item.Key);
                    SetPluginState(item.Key, PluginState.DepNotFound);
                    Logs.Error(string.Format("组件 {0} 加载失败，没有找到依赖 {1}", item, item1));
                    break;
                }

                if (item1.Type == "Load")
                {
                    try
                    {
                        item.Value.AddLoad(ass1);
                    }
                    catch (Exception e)
                    {
                        list.Add(item.Key);
                        SetPluginState(item.Key, PluginState.DepNotFound);
                        Logs.Error(string.Format("组件 {0} 加载依赖失败", item), e);
                    }
                }
                else if (item1.Type == "Share")
                {
                    item.Value.AddShare(ass1);
                }
            }

            try
            {
                item.Value.FindDll();
            }
            catch (Exception e)
            {
                list.Add(item.Key);
                SetPluginState(item.Key, PluginState.LoadError);
                Logs.Error(string.Format("组件 {0} 加载失败", item), e);
            }
        }


        foreach (var item in list.ToArray())
        {
            foreach (var item1 in PluginAssemblys.Values.Where(item2 =>
                item2.Obj.Dependents.Any(item3 => item3.ID == item)).ToArray())
            {
                list.Add(item1.Obj.ID);
            }
        }

        foreach (var item in list)
        {
            if (PluginAssemblys.Remove(item, out var ass))
            {
                ass.Unload();
            }

            ConfigHelper.Config.EnablePlugin.Remove(item);
        }
    }

    public static bool CheckOs(List<string> config)
    {
        if (config == null)
        {
            return true;
        }

        string system;
        if (SystemInfo.Os == OsType.Linux)
        {
            system = "linux_";
        }
        else if (SystemInfo.Os == OsType.MacOS)
        {
            system = "macos_";
        }
        else
        {
            system = "windows_";
        }

        if (SystemInfo.IsArm)
        {
            system += "arm64";
        }
        else
        {
            system += "x86_64";
        }

        return config.Contains(system);
    }

    /// <summary>
    /// 启用所有组件
    /// </summary>
    public static void StartPlugin()
    {
        foreach (var item in PluginAssemblys)
        {
            try
            {
                item.Value.Plugin.Init(item.Value.Local, InstanceManager.WorkDir);
                item.Value.Plugin.LoadLang(App.Lang);
                SetPluginState(item.Key, PluginState.Disable);
            }
            catch (Exception e)
            {
                ConfigHelper.Config.EnablePlugin.Remove(item.Key);
                item.Value.Enable = false;
                SetPluginState(item.Key, PluginState.EnableError);
                Logs.Error(string.Format("组件 {0} 初始化失败", item), e);
            }
        }

        var remove = new List<string>();
        foreach (var item in ConfigHelper.Config.EnablePlugin)
        {
            if (PluginAssemblys.TryGetValue(item, out var plugin))
            {
                try
                {
                    plugin.Plugin.Enable();
                    plugin.Enable = true;
                }
                catch (Exception e)
                {
                    plugin.Enable = false;
                    SetPluginState(item, PluginState.EnableError);
                    Logs.Error(string.Format("组件 {0} 启用失败", item), e);
                }
            }
            else
            {
                remove.Add(item);
            }
        }

        foreach (var item in remove)
        {
            ConfigHelper.Config.EnablePlugin.Remove(item);
        }
    }

    /// <summary>
    /// 停止所有组件
    /// </summary>
    public static void StopPlugin()
    {
        foreach (var item in PluginAssemblys)
        {
            StopPlugin(item.Key, item.Value.Plugin);
        }
    }

    /// <summary>
    /// 打开组件设置
    /// </summary>
    /// <param name="obj"></param>
    public static void OpenSetting(PluginDataObj obj)
    {
        if (PluginAssemblys.TryGetValue(obj.ID, out var value)
            && value.Plugin.HavePluginSetting)
        {
            var control = value.Plugin.OpenSetting();
            DialogHost.Show(new PluginSettingModel()
            {
                Control = control
            }, MainWindow.DialogHostName);
        }
    }

    /// <summary>
    /// 启用组件
    /// </summary>
    /// <param name="id"></param>
    public static void EnablePlugin(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var item))
        {
            try
            {
                item.Plugin.Enable();
                SetPluginState(id, PluginState.Enable);
                item.Enable = true;

                ConfigHelper.EnablePlugin(id);
                InstanceManager.EnablePlugin(id);
            }
            catch (Exception e)
            {
                item.Enable = false;
                SetPluginState(id, PluginState.EnableError);
                Logs.Error(string.Format("组件 {0} 启用失败", item), e);
                return;
            }
            try
            {
                LauncherHook.Instance?.PluginEnable(id);
            }
            catch
            { 
                
            }
        }
    }

    /// <summary>
    /// 禁用所有组件
    /// </summary>
    public static void DisablePlugin()
    {
        foreach (var item in PluginAssemblys)
        {
            ConfigHelper.DisablePlugin(item.Key);
            InstanceManager.DisablePlugin(item.Key);

            if (item.Value.Enable)
            {
                DisablePlugin(item.Key, item.Value.Plugin);
                item.Value.Enable = false;
            }
        }
    }

    /// <summary>
    /// 重载组件
    /// </summary>
    public static void Reload()
    {
        foreach (var item in PluginAssemblys)
        {
            InstanceManager.DisablePlugin(item.Key);
            if (item.Value.Enable)
            {
                DisablePlugin(item.Key, item.Value.Plugin);
                item.Value.Enable = false;
            }
            StopPlugin(item.Key, item.Value.Plugin);
            item.Value.Unload();
        }
        PluginDir.Clear();
        Plugins.Clear();
        PluginAssemblys.Clear();
        PluginStates.Clear();

        Init();
        StartPlugin();
        InstanceManager.StartInstance();

        try
        {
            LauncherHook.Instance?.PluginReload();
        }
        catch
        {

        }
    }

    /// <summary>
    /// 禁用组件
    /// </summary>
    /// <param name="id"></param>
    public static void DisablePlugin(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var item))
        {
            ConfigHelper.DisablePlugin(id);
            InstanceManager.DisablePlugin(id);

            DisablePlugin(id, item.Plugin);
            item.Enable = false;
            try
            {
                LauncherHook.Instance?.PluginDisable(id);
            }
            catch
            {

            }
        }
    }

    /// <summary>
    /// 组件是否启用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsEnable(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var item))
        {
            return item.Enable;
        }

        return false;
    }

    public static PluginState GetPluginState(string id)
    {
        if (PluginStates.TryGetValue(id, out var state))
        {
            return state;
        }

        return PluginState.LoadError;
    }

    public static bool HavePluginSetting(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var plugin))
        {
            return plugin.Plugin.HavePluginSetting;
        }

        return false;
    }

    public static bool IsCoreLib(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var plugin))
        {
            return plugin.Plugin.IsCoreLib;
        }

        return false;
    }

    public static bool HavePlugin(string id)
    {
        return Plugins.ContainsKey(id);
    }

    public static string? GetPluginVersion(string id)
    {
        if (Plugins.TryGetValue(id, out var obj))
        {
            return obj.Version;
        }

        return null;
    }

    public static string? DeletePlugin(PluginDownloadObj.ItemObj obj)
    {
        if (PluginDir.TryGetValue(obj.ID, out var dir))
        {
            Directory.Delete(dir, true);

            return dir;
        }

        return null;
    }

    public static async Task<bool> Download(PluginDownloadObj.ItemObj obj, string baseurl, string? dir = null)
    {
        dir ??= RunDir + "/" + obj.ID;
        int a = 1;
        while (Directory.Exists(dir))
        {
            dir = RunDir + "/" + obj.ID + $" ({a++})";
        }

        Directory.CreateDirectory(dir);

        foreach (var item in obj.Files)
        {
            var res = await TempManager.Download(baseurl + obj.Url + "/" + item.Name, dir + "/" + item.Name, item.Sha1);
            if (!res)
            {
                Directory.Delete(dir, true);
                return false;
            }
        }

        return true;
    }

    public static void AddControl(string id, string key, bool value)
    {
        if (!Controls.TryGetValue(id, out var temp))
        {
            temp = [];
        }
        if (!temp.TryAdd(key, value))
        {
            temp[key] = value;
        }
        Controls.TryAdd(id, temp);
    }

    public static void AddLib(string id, string key, bool share, List<string>? dlls)
    {
        if (PluginAssemblys.TryGetValue(id, out var plugin1)
            && PluginAssemblys.TryGetValue(key, out var plugin2))
        {
            if (share)
            {
                plugin1.AddShare(plugin2);
            }
            else if (dlls != null)
            {
                plugin1.AddLoad(plugin2, dlls);
            }
            else
            {
                plugin1.AddLoad(plugin2);
            }
        }
    }

    public static PluginDataObj Copy(this PluginDataObj obj)
    {
        var list = new List<PluginDependentObj>();
        foreach (var item in obj.Dependents)
        {
            list.Add(new()
            {
                Type = item.Type,
                ID = item.ID
            });
        }
        return new()
        {
            ID = obj.ID,
            Name = obj.Name,
            Auther = obj.Auther,
            Describe = obj.Describe,
            Dlls = [.. obj.Dlls],
            Dependents = list,
            Os = [.. obj.Os],
            Permission = obj.Permission,
            Version = obj.Version,
            ApiVersion = obj.ApiVersion
        };
    }

    private static void DisablePlugin(string id, IPlugin plugin)
    {
        try
        {
            plugin.Disable();
        }
        catch (Exception e)
        {
            SetPluginState(id, PluginState.EnableError);
            Logs.Error(string.Format("组件 {0} 禁用失败", id), e);
        }
    }

    private static void StopPlugin(string id, IPlugin plugin)
    {
        try
        {
            plugin.Stop();
        }
        catch (Exception e)
        {
            SetPluginState(id, PluginState.EnableError);
            Logs.Error(string.Format("组件 {0} 停止失败", id), e);
        }
    }

    /// <summary>
    /// 设置插件状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    private static void SetPluginState(string id, PluginState state)
    {
        if (!PluginStates.TryAdd(id, state))
        {
            PluginStates[id] = state;
        }
    }
}
