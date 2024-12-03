﻿using System;
using System.Collections.Generic;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Manager;

internal class LauncherHook : ILauncherHook
{
    public static LauncherHook Instance;

    private readonly List<Action<InstanceEvent>> _instanceEnableList = [];
    private readonly List<Action<InstanceEvent>> _instanceDisableList = [];
    private readonly List<Action<InstanceEvent>> _instanceCreateList = [];
    private readonly List<Action<InstanceEvent>> _instanceUpdateList = [];
    private readonly List<Action> _pluginReloadList = [];
    private readonly List<Action<PluginEvent>> _pluginEnableList = [];
    private readonly List<Action<PluginEvent>> _pluginDisableList = [];

    public event Action OnPluginReload
    {
        add
        {
            _pluginReloadList.Add(value);
        }
        remove
        {
            _pluginReloadList.Remove(value);
        }
    }
    public event Action<PluginEvent> OnPluginEnable
    {
        add
        {
            _pluginEnableList.Add(value);
        }
        remove
        {
            _pluginEnableList.Remove(value);
        }
    }
    public event Action<PluginEvent> OnPluginDisable
    {
        add
        {
            _pluginDisableList.Add(value);
        }
        remove
        {
            _pluginDisableList.Remove(value);
        }
    }
    public event Action<InstanceEvent> OnInstanceEnable 
    { 
        add
        {
            _instanceEnableList.Add(value);
        }
        remove
        {
            _instanceEnableList.Remove(value);
        }
    }
    public event Action<InstanceEvent> OnInstanceDisable
    {
        add
        {
            _instanceDisableList.Add(value);
        }
        remove
        {
            _instanceDisableList.Remove(value);
        }
    }
    public event Action<InstanceEvent> OnInstanceCreate
    {
        add
        {
            _instanceCreateList.Add(value);
        }
        remove
        {
            _instanceCreateList.Remove(value);
        }
    }
    public event Action<InstanceEvent> OnInstanceUpdate
    {
        add
        {
            _instanceUpdateList.Add(value);
        }
        remove
        {
            _instanceUpdateList.Remove(value);
        }
    }

    public LauncherHook()
    {
        Instance = this;
    }

    public void InstanceEnable(string plugin, string uuid)
    {
        foreach (var item in _instanceEnableList)
        {
            try
            {
                item.Invoke(new(plugin, uuid));
            }
            catch(Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void InstanceDisable(string plugin, string uuid)
    {
        foreach (var item in _instanceDisableList)
        {
            try
            {
                item.Invoke(new(plugin, uuid));
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void InstanceCreate(string plugin, string uuid)
    {
        foreach (var item in _instanceCreateList)
        {
            try
            {
                item.Invoke(new(plugin, uuid));
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void InstanceUpdate(string plugin, string uuid)
    {
        foreach (var item in _instanceUpdateList)
        {
            try
            {
                item.Invoke(new(plugin, uuid));
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void PluginReload()
    {
        foreach (var item in _pluginReloadList)
        {
            try
            {
                item.Invoke();
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void PluginEnable(string plugin)
    {
        foreach (var item in _pluginEnableList)
        {
            try
            {
                item.Invoke(new(plugin));
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public void PluginDisable(string plugin)
    {
        foreach (var item in _pluginDisableList)
        {
            try
            {
                item.Invoke(new(plugin));
            }
            catch (Exception e)
            {
                Logs.Error("Instance enable event crash", e);
            }
        }
    }

    public IInstanceManager GetInstanceManager(PluginDataObj obj, InstanceDataObj obj1)
    {
        return new InstanceHook(obj);
    }

    public IPluginManager GetPluginManager(PluginDataObj obj)
    {
        return new PluginHook(obj);
    }
}

public class PluginHook(PluginDataObj obj) : IPluginManager
{
    public ManagerState Disable(string key)
    {
        if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
        if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
            var res = plugin.Plugin.Permissions(obj.ID, permission);
            PluginManager.AddControl(obj.ID, key, res);
            return res;
        }
        else
        {
            PluginManager.AddControl(obj.ID, key, true);
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
        if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
            || !controls.TryGetValue(key, out var res))
        {
            return ManagerState.NoTestPermission;
        }

        if (!res)
        {
            return ManagerState.NoPermission;
        }

        PluginManager.AddLib(obj.ID, key, share, dlls);

        return ManagerState.Success;
    }
}

public class InstanceHook(PluginDataObj obj) : IInstanceManager
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
        if (obj.ID != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
        if (obj.ID != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
        if (obj.ID != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
        if (obj.ID != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
        if (obj.ID != data.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
            || !controls.TryGetValue(data.Plugin, out var res))
            {
                return ManagerState.NoTestPermission;
            }
            if (!res)
            {
                return ManagerState.NoPermission;
            }
        }

        run.Window.Update(data);
        data.Save();

        return ManagerState.Success;
    }

    public IInstanceHandel? GetHandel(string uuid)
    {
        if (!InstanceManager.RunInstances.TryGetValue(uuid, out var run))
        {
            return null;
        }
        if (obj.ID != run.InstanceData.Plugin)
        {
            if (!PluginManager.Controls.TryGetValue(obj.ID, out var controls)
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
}