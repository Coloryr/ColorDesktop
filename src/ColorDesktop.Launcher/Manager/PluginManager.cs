using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColorDesktop.Api;
using ColorDesktop.Launcher.Helper;
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
    public static readonly Dictionary<string, List<string>> Deps = [];

    /// <summary>
    /// 读取时失败
    /// </summary>
    public static readonly List<string> LoadError = [];
    /// <summary>
    /// 加载时失败
    /// </summary>
    public static readonly List<string> EnableError = [];

    public const string Dir1 = "plugins";

    public static string RunDir { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        RunDir = Path.GetFullPath(AppContext.BaseDirectory + Dir1);
        Directory.CreateDirectory(RunDir);

        var list1 = Directory.GetDirectories(RunDir);
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
                    if (Plugins.ContainsKey(obj.ID))
                    {
                        Logs.Error(string.Format("组件 {0} 存在重复的ID", item));
                        continue;
                    }

                    var ass = new PluginAssembly(config.DirectoryName!, obj);
                    PluginAssemblys.Add(obj.ID, ass);
                    Plugins.Add(obj.ID, obj);
                }
            }
            catch (Exception e)
            {
                LoadError.Add(item);
                Logs.Error(string.Format("组件 {0} 加载失败", item), e);
            }
        }

        foreach (var item in Plugins)
        {
            foreach (var item1 in item.Value.Dependents)
            {
                if (!PluginAssemblys.ContainsKey(item1))
                {
                    LoadError.Add(item.Key);
                    Logs.Error(string.Format("组件 {0} 加载失败，没有找到依赖 {1}", item, item1));
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

        foreach (var item in LoadError)
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
                item.Value.Plugin.Init(item.Value.Local, LanguageType.zh_cn);
            }
            catch (Exception e)
            {
                ConfigHelper.Config.EnablePlugin.Remove(item.Key);
                item.Value.Enable = false;
                AddEnableFail(item.Key);
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
                    AddEnableFail(item);
                    Logs.Error(string.Format("组件 {0} 启用失败", item), e);
                }
            }
            else
            {
                remove.Add(item);
            }
        }
        foreach (var item in EnableError)
        {
            ConfigHelper.Config.EnablePlugin.Remove(item);
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
                EnableError.Remove(id);
                item.Enable = true;

                ConfigHelper.EnablePlugin(id);
                InstanceManager.EnablePlugin(id);
            }
            catch (Exception e)
            {
                item.Enable = false;
                AddEnableFail(id);
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
        Deps.Clear();
        LoadError.Clear();
        EnableError.Clear();
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
        return EnableError.Contains(id);
    }

    /// <summary>
    /// 组件是否加载错误
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsFail(string id)
    {
        return LoadError.Contains(id);
    }

    public static bool HavePluginSetting(string id)
    {
        if (PluginAssemblys.TryGetValue(id, out var plugin))
        {
            return plugin.Plugin.HavePluginSetting;
        }

        return false;
    }

    private static void AddEnableFail(string id)
    {
        if (!EnableError.Contains(id))
        {
            EnableError.Add(id);
        }
    }

    private static void DisablePlugin(string id, IPlugin plugin)
    {
        try
        {
            plugin.Disable();
        }
        catch (Exception e)
        {
            AddEnableFail(id);
            Logs.Error(string.Format("组件 {0} 禁用失败", id), e);
        }
    }
}
