using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public interface ILauncherHook
{
    /// <summary>
    /// 组件重载
    /// </summary>
    event Action? OnPluginReload;
    /// <summary>
    /// 组件启用
    /// </summary>
    event Action<string>? OnPluginEnable;
    /// <summary>
    /// 组件禁用
    /// </summary>
    event Action<string>? OnPluginDisable;

    /// <summary>
    /// 获取实例控制器
    /// </summary>
    IInstanceManager GetInstanceManager(PluginDataObj obj, InstanceDataObj obj1);
    /// <summary>
    /// 获取组件控制器
    /// </summary>
    IPluginManager GetPluginManager(PluginDataObj obj);
}
