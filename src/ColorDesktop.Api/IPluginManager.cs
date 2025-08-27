using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

/// <summary>
/// 组件控制器
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// 获取组件列表
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<string> GetPlugins();
    /// <summary>
    /// 获取组件状态
    /// </summary>
    /// <param name="key">组件ID</param>
    /// <returns></returns>
    PluginState? GetPluginState(string key);
    /// <summary>
    /// 获取组件信息
    /// </summary>
    /// <param name="key">组件ID</param>
    /// <returns></returns>
    PluginDataObj? GetPluginData(string key);
    /// <summary>
    /// 获取组件是否已经启用
    /// </summary>
    /// <param name="key">组件ID</param>
    /// <returns></returns>
    bool? GetPluginEnable(string key);
    /// <summary>
    /// 请求控制组件
    /// </summary>
    /// <param name="key">组件ID</param>
    /// <param name="permission">权限</param>
    /// <returns>是否有权限控制</returns>
    bool? GetControlTest(string key, string permission);
    /// <summary>
    /// 禁用组件
    /// </summary>
    /// <param name="key">组件ID</param>
    ManagerState Disable(string key);
    /// <summary>
    /// 启用组件
    /// </summary>
    /// <param name="key">组件ID</param>
    ManagerState Enable(string key);
    /// <summary>
    /// 将目标组件的CLR加载到自身环境
    /// </summary>
    /// <param name="key">组件ID</param>
    /// <param name="share">共享的方式添加</param>
    /// <param name="dlls">指定需要加载的DLL</param>
    ManagerState InstallCLR(string key, bool share, List<string>? dlls = null);
}
