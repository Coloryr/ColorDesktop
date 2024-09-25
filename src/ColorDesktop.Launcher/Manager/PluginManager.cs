using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using ColorDesktop.Launcher.Utils;
using DialogHostAvalonia;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Manager;

public static class PluginManager
{
    public static readonly Dictionary<string, PluginDataObj> Plugins = [];
    public static readonly Dictionary<string, PluginAssembly> PluginAssemblys = [];
    public static readonly Dictionary<string, PluginState> PluginStates = [];

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
                    Logs.Error(string.Format("组件 {0} 存在重复的ID", item));
                    continue;
                }

                Plugins.Add(obj.ID, obj);

                if (obj.ApiVersion != Program.ApiVersion)
                {
                    Logs.Error(string.Format("组件 {0} 的API版本不一致", item));
                    list.Add(item);
                    SetPluginState(item, PluginState.LoadError);
                    continue;
                }

                PluginAssemblys.Add(obj.ID, new PluginAssembly(config.DirectoryName!, obj));
            }
            catch (Exception e)
            {
                list.Add(item);
                SetPluginState(item, PluginState.LoadError);
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
                    SetPluginState(item.Key, PluginState.LoadError);
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
                        SetPluginState(item.Key, PluginState.LoadError);
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

    /// <summary>
    /// 启用所有组件
    /// </summary>
    public static void StartPlugin()
    {
        foreach (var item in PluginAssemblys)
        {
            try
            {
                item.Value.Plugin.Init(item.Value.Local, InstanceManager.WorkDir, LanguageType.zh_cn);
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
            DisablePlugin(item.Key, item.Value.Plugin);
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

            DisablePlugin(item.Key, item.Value.Plugin);
            item.Value.Enable = false;
        }
    }

    /// <summary>
    /// 重载组件
    /// </summary>
    public static void Reload()
    {
        Plugins.Clear();
        foreach (var item in PluginAssemblys)
        {
            InstanceManager.DisablePlugin(item.Key);
            DisablePlugin(item.Key, item.Value.Plugin);
            item.Value.Unload();
        }
        PluginAssemblys.Clear();
        PluginStates.Clear();

        Init();
        StartPlugin();
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
            InstanceManager.DisablePlugin(id);

            DisablePlugin(id, item.Plugin);
            item.Enable = false;
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

    /// <summary>
    /// 组件是否启用错误
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsEnableFail(string id)
    {
        if (PluginStates.TryGetValue(id, out var state))
        {
            return state == PluginState.EnableError;
        }

        return false;
    }

    /// <summary>
    /// 组件是否加载错误
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsFail(string id)
    {
        if (PluginStates.TryGetValue(id, out var state))
        {
            return state is PluginState.LoadError or PluginState.DepNotFound;
        }

        return false;
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
