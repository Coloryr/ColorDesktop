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

    public static PluginAssembly WebPlugin;
    public static bool HaveWeb = false;

    public const string Dir1 = "plugins";
    public const string Dir2 = "WebPlugin";
    public const string ConfigName = "plugin.json";

    public static string RunDir { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init(bool reload = false)
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
                    if (reload)
                    {
                        continue;
                    }
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

        LoadWeb();
    }

    private static void LoadWeb()
    {
        if (!Directory.Exists(Program.RunDir + Dir2))
        {
            return;
        }

        try
        {
            var list2 = PathHelper.GetAllFile(Program.RunDir + Dir2);
            var config = list2.FirstOrDefault(item =>
                item.Name.Equals(ConfigName, StringComparison.CurrentCultureIgnoreCase));
            if (config == null)
            {
                return;
            }
            var obj = JsonConvert.DeserializeObject<PluginDataObj>(File.ReadAllText(config.FullName));
            if (obj == null)
            {
                return;
            }

            WebPlugin = new PluginAssembly(config.DirectoryName!, obj);
            WebPlugin.FindDll();
            HaveWeb = true;
        }
        catch (Exception e)
        {
            Logs.Error(string.Format("浏览器组件加载失败"), e);
        }
    }

    public static int GetReloadCount()
    {
        return Plugins.Count(item => item.Value.Reload == false);
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
    public static void StartPlugin(bool reload = false)
    {
        foreach (var item in PluginAssemblys)
        {
            if (reload && item.Value.Obj.Reload == false)
            {
                continue;
            }
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

        if (HaveWeb)
        {
            WebPlugin.Plugin.Init(WebPlugin.Local, InstanceManager.WorkDir);
        }

        var remove = new List<string>();
        foreach (var item in ConfigHelper.Config.EnablePlugin)
        {
            if (PluginAssemblys.TryGetValue(item, out var plugin))
            {
                if (reload && plugin.Obj.Reload == false)
                {
                    continue;
                }
                EnablePlugin(plugin);
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
    public static void StopPlugin(bool reload = false)
    {
        foreach (var item in PluginAssemblys)
        {
            if (reload && item.Value.Obj.Reload == false)
            {
                continue;
            }
            DisablePlugin(item.Value);
            StopPlugin(item.Value);
            item.Value.Unload();
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
            ConfigHelper.EnablePlugin(id);
            EnablePlugin(item);
        }
    }

    private static void EnablePlugin(PluginAssembly plugin)
    {
        try
        {
            plugin.Plugin.Enable();
            plugin.Enable = true;
            SetPluginState(plugin.Obj.ID, PluginState.Enable);
            
            InstanceManager.EnablePlugin(plugin.Obj.ID);
            LauncherHook.PluginEnable(plugin.Obj.ID);
        }
        catch (Exception e)
        {
            plugin.Enable = false;
            SetPluginState(plugin.Obj.ID, PluginState.EnableError);
            Logs.Error(string.Format("组件 {0} 启用失败", plugin.Obj.ID), e);
            return;
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
            DisablePlugin(item.Value);
        }
    }

    /// <summary>
    /// 重载组件
    /// </summary>
    public static void Reload()
    {
        LauncherHook.PluginReload();

        StopPlugin(true);

        foreach (var item in Plugins.ToArray())
        {
            if (item.Value.Reload == false)
            {
                continue;
            }
            PluginDir.Remove(item.Key);
            Plugins.Remove(item.Key);
            PluginAssemblys.Remove(item.Key);
            PluginStates.Remove(item.Key);
        }

        Init(true);
        StartPlugin(true);
        InstanceManager.StartInstance();
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
            DisablePlugin(item);
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

    private static void DisablePlugin(PluginAssembly plugin)
    {
        try
        {
            InstanceManager.DisablePlugin(plugin.Obj.ID);
            LauncherHook.PluginDisable(plugin.Obj.ID);
            if (plugin.Enable)
            {
                plugin.Enable = false;
                plugin.Plugin.Disable();
            }
        }
        catch (Exception e)
        {
            SetPluginState(plugin.Obj.ID, PluginState.EnableError);
            Logs.Error(string.Format("组件 {0} 禁用失败", plugin.Obj.ID), e);
        }
    }

    private static void StopPlugin(PluginAssembly plugin)
    {
        LauncherApi.RemoveListener(plugin.Obj.ID);
        try
        {
            plugin.Plugin.Stop();
        }
        catch (Exception e)
        {
            SetPluginState(plugin.Obj.ID, PluginState.EnableError);
            Logs.Error(string.Format("组件 {0} 停止失败", plugin.Obj.ID), e);
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
