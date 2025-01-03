namespace ColorDesktop.Api.Events;

/// <summary>
/// 组件禁用
/// </summary>
/// <param name="plugin"></param>
public class PluginDisableEvent(string plugin) : PluginEvent(plugin)
{

}
