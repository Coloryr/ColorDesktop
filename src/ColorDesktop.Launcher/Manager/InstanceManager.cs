using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AvaloniaEdit.Utils;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.Launcher.Helper;
using ColorDesktop.Launcher.Objs;
using ColorDesktop.Launcher.UI.Models.Dialog;
using ColorDesktop.Launcher.UI.Windows;
using DialogHostAvalonia;

namespace ColorDesktop.Launcher.Manager;

public static class InstanceManager
{
    public static readonly Dictionary<string, InstanceWindowObj> RunInstances = [];
    public static readonly Dictionary<string, InstanceState> InstanceStates = [];
    public static readonly Dictionary<string, GroupObj> Groups = [];

    public static readonly HashSet<string> KnowUUID = [];

    public static readonly Dictionary<string, InstanceDataObj> Instances = [];

    public const string Dir2 = "instances";
    public const string Die3 = "groups";
    public const string FileName = "instance.json";

    public static string WorkDir { get; private set; }
    public static string GroupDir { get; private set; }

    public static string? NowGroup { get; private set; }

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
                    var obj = JsonUtils.ToObj(File.ReadAllText(config.FullName), JsonType.InstanceDataObj);
                    if (obj == null)
                    {
                        continue;
                    }
                    if (obj.UUID != uuid)
                    {
                        obj.UUID = uuid;
                        ConfigUtils.Save(obj, config.FullName, JsonType.InstanceDataObj);
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

        LoadGroups();
    }

    private static void LoadGroups()
    {
        GroupDir = Path.GetFullPath(Program.RunDir + Die3);
        Directory.CreateDirectory(GroupDir);
        var info = new DirectoryInfo(GroupDir);

        foreach (var item in info.GetFiles())
        {
            try
            {
                var obj = JsonSerializer.Deserialize(File.ReadAllText(item.FullName), JsonType.GroupObj);
                if (obj != null && !string.IsNullOrWhiteSpace(obj.UUID)
                    && !string.IsNullOrWhiteSpace(obj.Name))
                {
                    obj.Instances ??= [];
                    obj.Enables ??= [];
                    KnowUUID.Add(obj.UUID);
                    Groups[obj.UUID] = obj;
                }
            }
            catch (Exception e)
            {
                Logs.Error("load group error", e);
            }
        }

        NowGroup = ConfigHelper.Config.Group;
    }

    /// <summary>
    /// 开始启用所有实例
    /// </summary>
    public static void StartInstance()
    {
        if (NowGroup != null && Groups.TryGetValue(NowGroup, out var group))
        {
            NowGroup = group.UUID;
            foreach (var item in group.Enables)
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
        }
        else
        {
            NowGroup = null;
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
        }

        App.ThisApp.UpdateMenu();
    }

    public static void CreateGroup(string name)
    {
        string uuid = MakeUUID();

        var obj = new GroupObj()
        {
            Name = name,
            UUID = uuid,
            Enables = [],
            Instances = []
        };

        Groups[uuid] = obj;
        obj.Save();

        LauncherHook.GroupcCreate(uuid);

        SwitchGroup(uuid);
    }

    public static void DeleteGroupUUID(string uuid)
    {
        if (string.IsNullOrWhiteSpace(uuid) || !Groups.ContainsKey(uuid))
        {
            return;
        }
        if (NowGroup == uuid)
        {
            SwitchGroup(null);
        }

        var file = Path.GetFullPath(GroupDir + "/" + uuid + ".json");
        if (File.Exists(file))
        {
            File.Delete(file);
        }

        Groups.Remove(uuid);

        LauncherHook.GroupDelete(uuid);
    }

    public static void Save(this GroupObj group)
    {
        var file = Path.GetFullPath(GroupDir + "/" + group.UUID + ".json");

        ConfigSave.AddItem(ConfigSaveObj.Build("Group " + group.UUID, file, group, JsonType.GroupObj));
    }

    public static void GroupImportInstance(string uuid, List<string> list)
    {
        if (Groups.TryGetValue(uuid, out var group))
        {
            group.Instances.AddRange(list);
            group.Save();
        }
    }

    public static void SwitchGroup(string? uuid)
    {
        var old = NowGroup;
        if (string.IsNullOrWhiteSpace(uuid))
        {
            NowGroup = null;
            foreach (var item in RunInstances)
            {
                if (!ConfigHelper.Config.EnableInstance.Contains(item.Key))
                {
                    StopInstance(item.Value);
                }
            }
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
        }
        else if (Groups.TryGetValue(uuid, out var group))
        {
            NowGroup = uuid;
            foreach (var item in RunInstances)
            {
                if (!group.Enables.Contains(item.Key))
                {
                    StopInstance(item.Value);
                }
            }
            foreach (var item in group.Enables)
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
        }

        LauncherHook.GroupSwitch(old, NowGroup);

        ConfigHelper.Config.Group = NowGroup;
        ConfigHelper.SaveConfig();
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

            LauncherHook.InstanceCreate(config.Plugin, NowGroup, config.UUID);
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
        ConfigUtils.Save(obj, Path.GetFullPath(dir + "/" + FileName), JsonType.InstanceDataObj);
    }

    /// <summary>
    /// 拷贝分组
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static GroupObj Copy(this GroupObj obj)
    {
        return new GroupObj()
        {
            Name = obj.Name,
            UUID = obj.UUID,
            Instances = [.. obj.Instances],
            Enables = [.. obj.Enables]
        };
    }

    /// <summary>
    /// 创建一个实例
    /// </summary>
    /// <param name="obj"></param>
    private static void CreateInstance(InstanceDataObj obj)
    {
        obj.Save();
        Instances.Add(obj.UUID, obj);
        if (NowGroup != null && Groups.TryGetValue(NowGroup, out var group))
        {
            group.Instances.Add(obj.UUID);
            group.Enables.Add(obj.UUID);
            group.Save();
        }
        else
        {
            ConfigHelper.EnableInstance(obj.UUID);
        }
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
        }
    }

    /// <summary>
    /// 停止实例
    /// </summary>
    /// <param name="instance"></param>
    private static void StopInstance(InstanceWindowObj instance)
    {
        try
        {
            LauncherHook.InstanceDisable(instance.InstanceData.Plugin, NowGroup, instance.InstanceData.UUID);

            RunInstances.Remove(instance.InstanceData.UUID);

            instance.Instance.Stop(instance.Window);
            instance.Window.Close();

            LangSel.Remove();
        }
        catch (Exception e)
        {
            Logs.Error(string.Format("实例 {0} 停止失败", instance.InstanceData.UUID), e);
        }

        SetInstanceState(instance.InstanceData.UUID, InstanceState.Disable);
    }

    /// <summary>
    /// 启用该组件的所有实例
    /// </summary>
    /// <param name="id"></param>
    public static void EnablePlugin(string id)
    {
        var list = Instances.Where(item => item.Value.Plugin == id).Select(item => item.Value).ToList();

        if (NowGroup != null && Groups.TryGetValue(NowGroup, out var group))
        {
            foreach (var item in list)
            {
                if (group.Enables.Contains(item.UUID))
                {
                    StartInstance(item);
                }
            }
        }
        else
        {
            foreach (var item in list)
            {
                if (ConfigHelper.Config.EnableInstance.Contains(item.UUID))
                {
                    StartInstance(item);
                }
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
        if (NowGroup != null && Groups.TryGetValue(NowGroup, out var group))
        {
            group.Instances.Add(obj.UUID);
            group.Enables.Add(obj.UUID);
            group.Save();
        }
        else
        {
            ConfigHelper.EnableInstance(obj.UUID);
        }

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
        if (NowGroup != null && Groups.TryGetValue(NowGroup, out var group))
        {
            group.Enables.Remove(obj.UUID);
            group.Save();
        }
        else
        {
            ConfigHelper.DisableInstance(obj.UUID);
        }

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
            run.Window.Update(obj);
            obj.Save();

            LauncherHook.InstanceUpdate(obj.Plugin, NowGroup, obj.UUID);
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
        KnowUUID.Add(uuid);
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

                LauncherHook.InstanceEnable(obj.Plugin, NowGroup, obj.UUID);
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

        foreach (var item in Groups.Values)
        {
            item.Instances.Remove(uuid);
            item.Enables.Remove(uuid);
            item.Save();
        }

        ConfigHelper.DisableInstance(uuid);

        LauncherHook.InstanceDelete(plugin, NowGroup, uuid);

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
