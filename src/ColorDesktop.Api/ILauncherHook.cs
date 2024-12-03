namespace ColorDesktop.Api;

public interface ILauncherHook
{
    /// <summary>
    /// 组件重载
    /// </summary>
    event Action OnPluginReload;
    /// <summary>
    /// 组件启用
    /// </summary>
    event Action<PluginEvent> OnPluginEnable;
    /// <summary>
    /// 组件禁用
    /// </summary>
    event Action<PluginEvent> OnPluginDisable;

    /// <summary>
    /// 实例启用
    /// </summary>
    event Action<InstanceEvent> OnInstanceEnable;
    /// <summary>
    /// 实例禁用
    /// </summary>
    event Action<InstanceEvent> OnInstanceDisable;
    /// <summary>
    /// 实例创建
    /// </summary>
    event Action<InstanceEvent> OnInstanceCreate;
    /// <summary>
    /// 实例更新配置
    /// </summary>
    event Action<InstanceEvent> OnInstanceUpdate;

    /// <summary>
    /// 获取实例控制器
    /// </summary>
    IInstanceManager GetInstanceManager(PluginDataObj obj, InstanceDataObj obj1);
    /// <summary>
    /// 获取组件控制器
    /// </summary>
    IPluginManager GetPluginManager(PluginDataObj obj);
}
