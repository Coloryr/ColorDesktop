namespace ColorDesktop.Api.Events;

/// <summary>
/// 组件启用
/// </summary>
/// <param name="plugin"></param>
public class PluginEnableEvent(string plugin) : PluginEvent(plugin)
{

}
