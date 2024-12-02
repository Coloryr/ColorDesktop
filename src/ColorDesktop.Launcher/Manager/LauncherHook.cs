using System;
using System.Collections.Generic;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Manager;

internal class LauncherHook : ILauncherHook
{
    public static LauncherHook Instance;

    public event Action? OnPluginReload;
    public event Action<string>? OnPluginEnable;
    public event Action<string>? OnPluginDisable;

    public LauncherHook()
    {
        Instance = this;
    }

    public void PluginReload()
    {
        OnPluginReload?.Invoke();
    }

    public void PluginEnable(string key)
    {
        OnPluginEnable?.Invoke(key);
    }

    public void PluginDisable(string key)
    {
        OnPluginDisable?.Invoke(key);
    }

    public IInstanceManager GetInstanceManager(PluginDataObj obj, InstanceDataObj obj1)
    {
        return new InstanceHook(obj, obj1);
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
        if (PluginManager.Controls.TryGetValue(obj.ID, out var controls)
            && controls.TryGetValue(key, out var res))
        {
            if (res)
            {
                if (PluginManager.IsEnable(key))
                {
                    PluginManager.DisablePlugin(key);
                    return ManagerState.Success;
                }

                return ManagerState.IsDisabled;
            }
            else
            {
                return ManagerState.NoPermission;
            }
        }
        else
        {
            return ManagerState.NoTestPermission;
        }
    }

    public ManagerState Enable(string key)
    {
        if (PluginManager.Controls.TryGetValue(obj.ID, out var controls)
            && controls.TryGetValue(key, out var res))
        {
            if (res)
            {
                if (!PluginManager.IsEnable(key))
                {
                    PluginManager.DisablePlugin(key);
                    return ManagerState.Success;
                }

                return ManagerState.IsEnabled;
            }
            else
            {
                return ManagerState.NoPermission;
            }
        }
        else
        {
            return ManagerState.NoTestPermission;
        }
    }

    public bool? GetControlTest(string key, string permission)
    {
        if (PluginManager.Plugins.TryGetValue(key, out var data)
            && PluginManager.PluginAssemblys.TryGetValue(key, out var plugin))
        {
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
        else
        {
            return null;
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
        if (PluginManager.Controls.TryGetValue(obj.ID, out var controls)
            && controls.TryGetValue(key, out var res))
        {
            if (res)
            {
                PluginManager.AddLib(obj.ID, key, share, dlls);

                return ManagerState.Success;
            }
            else
            {
                return ManagerState.NoPermission;
            }
        }
        else
        {
            return ManagerState.NoTestPermission;
        }
    }
}

public class InstanceHook(PluginDataObj obj, InstanceDataObj obj1) : IInstanceManager
{
    public IReadOnlyList<string> GetInstances()
    {
        return [.. InstanceManager.Instances.Keys];
    }

    public InstanceDataObj? GetInstanceData(string key)
    {
        if (InstanceManager.Instances.TryGetValue(key, out var value))
        {
            return value.Copy();
        }

        return null;
    }

    public InstanceState? GetState(string key)
    {
        if (InstanceManager.InstanceStates.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }
}