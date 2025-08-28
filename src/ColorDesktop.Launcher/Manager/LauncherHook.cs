using System.Collections.Generic;
using ColorDesktop.Api;
using ColorDesktop.Api.Events;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Launcher.Manager;

internal class LauncherHook : ILauncherHook
{
    public static void GroupcCreate(string uuid)
    {
        LauncherApi.CallEvent(new GroupAddEvent(uuid));
    }

    public static void GroupDelete(string uuid)
    {
        LauncherApi.CallEvent(new GroupDeleteEvent(uuid));
    }

    public static void GroupSwitch(string? old, string? uuid)
    {
        LauncherApi.CallEvent(new GroupSwitchEvent(old, uuid));
    }

    public static void InstanceEnable(string plugin, string? group, string uuid)
    {
        LauncherApi.CallEvent(new InstanceEnableEvent(plugin, group, uuid));
    }

    public static void InstanceDisable(string plugin, string? group, string uuid)
    {
        LauncherApi.CallEvent(new InstanceDisableEvent(plugin, group, uuid));
    }

    public static void InstanceCreate(string plugin, string? group, string uuid)
    {
        LauncherApi.CallEvent(new InstanceCreateEvent(plugin, group, uuid));
    }

    public static void InstanceUpdate(string plugin, string? group, string uuid)
    {
        LauncherApi.CallEvent(new InstanceUpdateEvent(plugin, group, uuid));
    }

    public static void InstanceDelete(string plugin, string? group, string uuid)
    {
        LauncherApi.CallEvent(new InstanceDeleteEvent(plugin, group, uuid));
    }

    public static void PluginReload()
    {
        LauncherApi.CallEvent(new PluginReloadEvent());
    }

    public static void PluginEnable(string plugin)
    {
        LauncherApi.CallEvent(new PluginEnableEvent(plugin));
    }

    public static void PluginDisable(string plugin)
    {
        LauncherApi.CallEvent(new PluginDisableEvent(plugin));
    }

    public IInstanceManager? GetInstanceManager(IPlugin obj)
    {
        foreach (var item in PluginManager.PluginAssemblys.Values)
        {
            if (item.Plugin == obj)
            {
                return new InstanceHook(item.Obj.ID);
            }
        }
        return null;
    }

    public IPluginManager? GetPluginManager(IPlugin obj)
    {
        foreach (var item in PluginManager.PluginAssemblys.Values)
        {
            if (item.Plugin == obj)
            {
                return new PluginHook(item.Obj.ID);
            }
        }
        return null;
    }

    public string? GetPluginId(IPlugin obj)
    {
        foreach (var item in PluginManager.PluginAssemblys.Values)
        {
            if (item.Plugin == obj)
            {
                return item.Obj.ID;
            }
        }
        return null;
    }
}

public class PluginHook(string id) : IPluginManager
{
    public ManagerState Disable(string key)
    {
        if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(key, out var res))
        {
            return ManagerState.NoTestPermission;
        }

        if (!res)
        {
            return ManagerState.NoPermission;
        }

        if (PluginManager.IsEnable(key))
        {
            PluginManager.DisablePlugin(key);
            return ManagerState.Success;
        }

        return ManagerState.PluginIsDisabled;
    }

    public ManagerState Enable(string key)
    {
        if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(key, out var res))
        {
            return ManagerState.NoTestPermission;
        }

        if (!res)
        {
            return ManagerState.NoPermission;
        }

        if (!PluginManager.IsEnable(key))
        {
            PluginManager.DisablePlugin(key);
            return ManagerState.Success;
        }

        return ManagerState.PluginIsEnabled;
    }

    public bool? GetControlTest(string key, string permission)
    {
        if (!PluginManager.Plugins.TryGetValue(key, out var data)
            || !PluginManager.PluginAssemblys.TryGetValue(key, out var plugin))
        {
            return null;
        }

        if (data.Permission == true)
        {
            var res = plugin.Plugin.Permissions(id, permission);
            PluginManager.AddControl(id, key, res);
            return res;
        }
        else
        {
            PluginManager.AddControl(id, key, true);
            return true;
        }
    }

    public PluginDataObj? GetPluginData(string key)
    {
        if (PluginManager.Plugins.TryGetValue(key, out var data))
        {
            return data.Copy();
        }

        return null;
    }

    public bool? GetPluginEnable(string key)
    {
        if (PluginManager.PluginAssemblys.TryGetValue(key, out var value))
        {
            return value.Enable;
        }

        return null;
    }

    public IReadOnlyList<string> GetPlugins()
    {
        return [.. PluginManager.Plugins.Keys];
    }

    public PluginState? GetPluginState(string key)
    {
        if (PluginManager.PluginStates.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }

    public ManagerState InstallCLR(string key, bool share, List<string>? dlls = null)
    {
        if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(key, out var res))
        {
            return ManagerState.NoTestPermission;
        }

        if (!res)
        {
            return ManagerState.NoPermission;
        }

        PluginManager.AddLib(id, key, share, dlls);

        return ManagerState.Success;
    }
}

public class InstanceHook(string id) : IInstanceManager
{
    public IReadOnlyList<string> GetInstances()
    {
        return [.. InstanceManager.Instances.Keys];
    }

    public InstanceDataObj? GetInstanceData(string uuid)
    {
        if (InstanceManager.Instances.TryGetValue(uuid, out var value))
        {
            return value.Copy();
        }

        return null;
    }

    public InstanceState? GetState(string uuid)
    {
        if (InstanceManager.InstanceStates.TryGetValue(uuid, out var value))
        {
            return value;
        }

        return null;
    }

    public ManagerState Enable(string uuid)
    {
        if (!InstanceManager.Instances.TryGetValue(uuid, out var data))
        {
            return ManagerState.InstanceNotFound;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
                || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        if (InstanceManager.RunInstances.TryGetValue(uuid, out _))
        {
            return ManagerState.InstanceIsEnabled;
        }

        InstanceManager.EnableInstance(data);

        return ManagerState.Success;
    }

    public ManagerState Disable(string uuid)
    {
        if (!InstanceManager.Instances.TryGetValue(uuid, out var data))
        {
            return ManagerState.InstanceNotFound;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        if (!InstanceManager.RunInstances.TryGetValue(uuid, out _))
        {
            return ManagerState.InstanceIsDisabled;
        }

        InstanceManager.DisableInstance(data);

        return ManagerState.Success;
    }

    public ManagerState Delete(string uuid)
    {
        if (!InstanceManager.Instances.TryGetValue(uuid, out var data))
        {
            return ManagerState.InstanceNotFound;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        if (!InstanceManager.RunInstances.TryGetValue(uuid, out _))
        {
            return ManagerState.InstanceIsDisabled;
        }

        InstanceManager.DisableInstance(data);

        return ManagerState.Success;
    }

    public ManagerState Create(InstanceDataObj data)
    {
        if (!PluginManager.PluginAssemblys.TryGetValue(data.Plugin, out var plugin))
        {
            return ManagerState.PluginNotFound;
        }
        if (!plugin.Enable)
        {
            return ManagerState.PluginIsDisabled;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        InstanceManager.EnableInstance(data);

        return ManagerState.Success;
    }

    public ManagerState SetInstanceData(InstanceDataObj data)
    {
        if (!InstanceManager.Instances.TryGetValue(data.UUID, out _))
        {
            return ManagerState.InstanceNotFound;
        }
        if (!InstanceManager.RunInstances.TryGetValue(data.UUID, out var run))
        {
            return ManagerState.InstanceIsDisabled;
        }
        if (!PluginManager.PluginAssemblys.TryGetValue(data.Plugin, out var plugin))
        {
            return ManagerState.PluginNotFound;
        }
        if (!plugin.Enable)
        {
            return ManagerState.PluginIsDisabled;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        run.Instance.Update(data);
        data.Save();

        return ManagerState.Success;
    }

    public IInstanceHandel? GetHandel(string uuid)
    {
        if (!InstanceManager.RunInstances.TryGetValue(uuid, out var run))
        {
            return null;
        }
        if (id != run.InstanceData.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
            || !controls.TryGetValue(run.InstanceData.Plugin, out var res))
            {
                return null;
            }
            if (!res)
            {
                return null;
            }
        }

        return run.Instance.GetHandel();
    }

    public IReadOnlyList<string> GetGroups()
    {
        return [.. InstanceManager.Groups.Keys];
    }

    public string? GetNowGroup()
    {
        return InstanceManager.NowGroup;
    }

    public void SwitchGroup(string? uuid)
    {
        InstanceManager.SwitchGroup(uuid);
    }

    public GroupObj? GetGroupObj(string uuid)
    {
        if (string.IsNullOrWhiteSpace(uuid))
        {
            return null;
        }
        if (InstanceManager.Groups.TryGetValue(uuid, out var group))
        {
            return group.Copy();
        }

        return null;
    }

    public ManagerState EditGroup(string uuid, GroupEditType type)
    {
        if (InstanceManager.NowGroup == null)
        {
            return ManagerState.Fail;
        }
        if (!InstanceManager.Instances.TryGetValue(uuid, out var data))
        {
            return ManagerState.InstanceNotFound;
        }
        if (id != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(id, out var controls)
                || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        var group = InstanceManager.Groups[InstanceManager.NowGroup];

        if (type == GroupEditType.Add)
        {
            group.Instances.Add(uuid);
        }
        else if (type == GroupEditType.Remove)
        {
            group.Enables.Remove(uuid);
            group.Instances.Remove(uuid);
        }
        else if (type == GroupEditType.Enable)
        {
            group.Instances.Add(uuid);
            group.Enables.Add(uuid);
        }
        else if (type == GroupEditType.Disable)
        {
            group.Enables.Remove(uuid);
        }

        return ManagerState.Success;
    }

    public string? CreateGroup(string name)
    {
        InstanceManager.CreateGroup(name);
        return InstanceManager.NowGroup;
    }

    public ManagerState DeleteGroup(string uuid)
    {
        if (string.IsNullOrWhiteSpace(uuid))
        {
            return ManagerState.Fail;
        }
        if (!InstanceManager.Groups.TryGetValue(uuid, out var group))
        {
            return ManagerState.Fail;
        }
        foreach (var item in group.Instances)
        {
            if (!InstanceManager.Instances.TryGetValue(item, out var data))
            {
                return ManagerState.InstanceNotFound;
            }
            if (id != data.Plugin)
            {
                if (!PluginManager.Controls.TryGetValue(id, out var controls)
                    || !controls.TryGetValue(data.Plugin, out var res))
                {
                    return ManagerState.NoTestPermission;
                }
                if (!res)
                {
                    return ManagerState.NoPermission;
                }
            }
        }

        InstanceManager.DeleteGroupUUID(uuid);

        return ManagerState.Success;
    }
}