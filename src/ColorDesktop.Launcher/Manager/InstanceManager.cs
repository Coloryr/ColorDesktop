﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using DialogHostAvalonia;
using Newtonsoft.Json;

namespace ColorDesktop.Launcher.Manager;

public static class InstanceManager
{
    public static readonly Dictionary<string, InstanceWindowObj> RunInstances = [];
    public static readonly Dictionary<string, InstanceState> InstanceStates = [];

    public static readonly HashSet<string> KnowUUID = [];

    public static readonly Dictionary<string, InstanceDataObj> Instances = [];

    public const string Dir2 = "instances";
    public const string FileName = "instance.json";

    public static string WorkDir { get; private set; }

    /// <summary>
    /// 加载所有实例
    /// </summary>
    public static void Init()
    {
        WorkDir = Path.GetFullPath(Program.RunDir + Dir2);
        Directory.CreateDirectory(WorkDir);
        var info = new DirectoryInfo(WorkDir);

        var list = new List<string>();

        foreach (var item in info.GetDirectories())
        {
            var uuid = item.Name;
            try
            {
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
                    SetInstanceState(uuid, InstanceState.Disable);
                }
            }
            catch (Exception e)
            {
                list.Add(uuid);
                SetInstanceState(uuid, InstanceState.LoadError);
                Logs.Error(string.Format("实例 {0} 加载失败", item.Name), e);
            }
        }

        foreach (var item in Instances)
        {
            if (!PluginManager.PluginAssemblys.ContainsKey(item.Value.Plugin))
            {
                list.Add(item.Key);
                SetInstanceState(item.Key, InstanceState.PluginNotFound);
                Logs.Error(string.Format("实例 {0} 没有找到组件 {1}", item.Key, item.Value.Plugin));
            }
        }

        foreach (var item1 in list)
        {
            ConfigHelper.Config.EnableInstance.Remove(item1);
        }
    }

    /// <summary>
    /// 开始启用所有实例
    /// </summary>
    public static void StartInstance()
    {
        foreach (var item in ConfigHelper.Config.EnableInstance)
        {
            if (Instances.TryGetValue(item, out var obj))
            {
                if (RunInstances.ContainsKey(item))
                {
                    continue;
                }
                StartInstance(obj);
            }
        }

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 新建一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    public static async void CreateInstance(PluginDataObj obj)
    {
        if (PluginManager.PluginAssemblys.TryGetValue(obj.ID, out var value))
        {
            var config = value.Plugin.CreateInstanceDefault();
            config.UUID = MakeUUID();
            config.Save();
            InstanceSetting? control = null;
            if (value.Plugin.HaveInstanceSetting)
            {
                control = value.Plugin.OpenSetting(config, true);
            }

            if (control?.Control != null)
            {
                var config1 = await DialogHost.Show(
                    new CreateInstanceOtherModel(config, control.Control),
                    MainWindow.DialogHostName);
                if (config1 is not true)
                {
                    Delete(config);
                    return;
                }
            }
            else
            {
                var config1 = await DialogHost.Show(new CreateInstanceOneModel(config), MainWindow.DialogHostName);
                if (config1 is not true)
                {
                    Delete(config);
                    return;
                }
            }

            control?.Close?.Invoke();

            CreateInstance(config);
            App.MainWindow?.LoadInstance();
            StartInstance(config);

            App.ThisApp.UpdateMenu();

            LauncherHook.InstanceCreate(config.Plugin, config.UUID);
        }
    }

    /// <summary>
    /// 保存实例设置
    /// </summary>
    /// <param name="obj"></param>
    public static void Save(this InstanceDataObj obj)
    {
        var dir = GetLocal(obj);
        Directory.CreateDirectory(dir);
        ConfigUtils.Save(obj, Path.GetFullPath(dir + "/" + FileName));
    }

    /// <summary>
    /// 创建一个实例
    /// </summary>
    /// <param name="obj"></param>
    private static void CreateInstance(InstanceDataObj obj)
    {
        obj.Save();
        Instances.Add(obj.UUID, obj);
        KnowUUID.Add(obj.UUID);
        ConfigHelper.EnableInstance(obj.UUID);
    }

    /// <summary>
    /// 获取工作路径
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static string GetLocal(InstanceDataObj obj)
    {
        return Path.GetFullPath(WorkDir + "/" + obj.UUID);
    }

    /// <summary>
    /// 停止所有实例
    /// </summary>
    public static void StopInstance()
    {
        foreach (var item in RunInstances.Values.ToArray())
        {
            StopInstance(item);
        }
    }

    /// <summary>
    /// 停止该实例
    /// </summary>
    /// <param name="uuid"></param>
    private static void StopInstance(string uuid)
    {
        if (RunInstances.TryGetValue(uuid, out var instance))
        {
            StopInstance(instance);

            SetInstanceState(uuid, InstanceState.Disable);
        }
    }

    /// <summary>
    /// 停止改实例
    /// </summary>
    /// <param name="instance"></param>
    private static void StopInstance(InstanceWindowObj instance)
    {
        try
        {
            LauncherHook.InstanceDisable(instance.InstanceData.Plugin, instance.InstanceData.UUID);

            RunInstances.Remove(instance.InstanceData.UUID);

            instance.Instance.Stop(instance.Window);
            instance.Window.Close();

            LangSel.Remove();
        }
        catch (Exception e)
        {
            Logs.Error(string.Format("实例 {0} 停止失败", instance.InstanceData.UUID), e);
        }
    }

    /// <summary>
    /// 启用该组件的所有实例
    /// </summary>
    /// <param name="id"></param>
    public static void EnablePlugin(string id)
    {
        var list = Instances.Where(item => item.Value.Plugin == id).Select(item => item.Value).ToList();

        foreach (var item in list)
        {
            if (ConfigHelper.Config.EnableInstance.Contains(item.UUID))
            {
                StartInstance(item);
            }
        }

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 禁用该组件的所有实例
    /// </summary>
    /// <param name="id"></param>
    public static void DisablePlugin(string id)
    {
        var list = RunInstances.Where(item => item.Value.InstanceData.Plugin == id).ToList();

        foreach (var item in list)
        {
            StopInstance(item.Value);
        }

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 启用一个实例
    /// </summary>
    /// <param name="obj"></param>
    public static void EnableInstance(InstanceDataObj obj)
    {
        ConfigHelper.EnableInstance(obj.UUID);

        StartInstance(obj);

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 禁用所有实例
    /// </summary>
    public static void DisableInstance()
    {
        foreach (var item in RunInstances.ToArray())
        {
            ConfigHelper.DisableInstance(item.Key);

            StopInstance(item.Value);
        }

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 禁用实例
    /// </summary>
    /// <param name="obj"></param>
    public static void DisableInstance(InstanceDataObj obj)
    {
        ConfigHelper.DisableInstance(obj.UUID);

        StopInstance(obj.UUID);

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 打开实例设置
    /// </summary>
    /// <param name="obj"></param>
    public static async Task OpenSetting(InstanceDataObj obj)
    {
        if (RunInstances.TryGetValue(obj.UUID, out var run)
            && PluginManager.PluginAssemblys.TryGetValue(obj.Plugin, out var plugin))
        {
            InstanceSetting? control = null;
            if (plugin.Plugin.HaveInstanceSetting)
            {
                control = plugin.Plugin.OpenSetting(obj, false);
            }
            if (control?.Control != null)
            {
                await DialogHost.Show(new CreateInstanceOtherModel(obj, control.Control)
                {
                    HaveCancel = false
                },
                MainWindow.DialogHostName);
            }
            else
            {
                await DialogHost.Show(new CreateInstanceOneModel(obj)
                {
                    HaveCancel = false
                }, MainWindow.DialogHostName);
            }

            control?.Close?.Invoke();

            run.Instance.Update(obj);
            obj.Save();

            LauncherHook.InstanceUpdate(obj.Plugin, obj.UUID);
        }
    }

    /// <summary>
    /// 实例是否已经启用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsEnable(string id)
    {
        if (InstanceStates.TryGetValue(id, out var state))
        {
            return state != InstanceState.Disable;
        }

        return false;
    }

    /// <summary>
    /// 实例是否启用失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsEnableFail(string id)
    {
        if (InstanceStates.TryGetValue(id, out var state))
        {
            return state == InstanceState.LoadFail;
        }

        return false;
    }

    /// <summary>
    /// 设置实例状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    private static void SetInstanceState(string id, InstanceState state)
    {
        if (!InstanceStates.TryAdd(id, state))
        {
            InstanceStates[id] = state;
        }
    }

    /// <summary>
    /// 创建一个实例UUID
    /// </summary>
    /// <returns></returns>
    private static string MakeUUID()
    {
        string uuid;
        do
        {
            uuid = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }
        while (KnowUUID.Contains(uuid));

        return uuid;
    }

    /// <summary>
    /// 开启一个显示实例
    /// </summary>
    /// <param name="obj"></param>
    private static void StartInstance(InstanceDataObj obj)
    {
        try
        {
            if (PluginManager.PluginAssemblys.TryGetValue(obj.Plugin, out var value))
            {
                if (!value.Enable)
                {
                    SetInstanceState(obj.UUID, InstanceState.PluginDisable);
                    return;
                }
                var view = value.Plugin.MakeInstances(obj);
                IInstanceWindow window;
                if (obj.IsWindow)
                {
                    window = (view.CreateView() as IInstanceWindow)!;
                }
                else
                {
                    var window1 = new InstanceWindow();
                    window1.Load(view, obj);
                    window = window1;
                }

                RunInstances.Add(obj.UUID, new(window, view, obj));
                window.Show();

                SetInstanceState(obj.UUID, InstanceState.Enable);

                LauncherHook.InstanceEnable(obj.Plugin, obj.UUID);
            }
            else
            {
                SetInstanceState(obj.UUID, InstanceState.Disable);
            }
        }
        catch (Exception e)
        {
            SetInstanceState(obj.UUID, InstanceState.LoadFail);
            Logs.Error(string.Format("实例 {0} 加载失败", obj.UUID), e);
        }
    }

    /// <summary>
    /// 删除实例配置
    /// </summary>
    /// <param name="obj"></param>
    private static void Delete(InstanceDataObj obj)
    {
        var dir = new DirectoryInfo(Path.GetFullPath(WorkDir + "/" + obj.UUID));
        if (dir.Exists)
        {
            dir.Delete(true);
        }
    }

    /// <summary>
    /// 删除一个实例
    /// </summary>
    /// <param name="uuid"></param>
    public static void Delete(string plugin, string uuid)
    {
        try
        {
            StopInstance(uuid);
        }
        catch (Exception e)
        {
            Logs.Error(string.Format("停止实例 {0} 错误", uuid), e);
        }
        var dir = new DirectoryInfo(Path.GetFullPath(WorkDir + "/" + uuid));
        if (dir.Exists)
        {
            dir.Delete(true);
        }

        LauncherHook.InstanceDelete(plugin, uuid);

        InstanceStates.Remove(uuid);
        Instances.Remove(uuid);
        KnowUUID.Remove(uuid);

        App.ThisApp.UpdateMenu();
    }

    /// <summary>
    /// 重载所有实例
    /// </summary>
    public static void Reload()
    {
        foreach (var item in RunInstances.Values.ToArray())
        {
            StopInstance(item);
        }
        RunInstances.Clear();
        InstanceStates.Clear();
        Instances.Clear();
        KnowUUID.Clear();
        Init();
        StartInstance();
    }

    public static void Move()
    {
        foreach (var item in RunInstances.Values)
        {
            if (item.Window is InstanceWindow window)
            {
                window.Move();
            }
        }
    }

    public static InstanceDataObj Copy(this InstanceDataObj obj)
    {
        return new()
        {
            Plugin = obj.Plugin,
            UUID = obj.UUID,
            Nick = obj.Nick,
            Pos = obj.Pos,
            Margin = new(obj.Margin),
            Tran = obj.Tran,
            Display = obj.Display,
            IsWindow = obj.IsWindow,
            TopModel = obj.TopModel
        };
    }
}
