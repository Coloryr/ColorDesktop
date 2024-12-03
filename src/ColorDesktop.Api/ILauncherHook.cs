namespace ColorDesktop.Api;

public interface ILauncherHook
{
    /// <summary>
    /// 获取实例控制器
    /// </summary>
    IInstanceManager? GetInstanceManager(IPlugin obj, IInstance obj1);
    /// <summary>
    /// 获取组件控制器
    /// </summary>
    IPluginManager? GetPluginManager(IPlugin obj);

    string? GetPluginId(IPlugin obj);
}
