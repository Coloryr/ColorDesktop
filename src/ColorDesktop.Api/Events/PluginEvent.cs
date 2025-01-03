namespace ColorDesktop.Api.Events;

/// <summary>
/// 组件事件
/// </summary>
/// <param name="plugin"></param>
public abstract class PluginEvent(string plugin) : BaseEvent
{
    /// <summary>
    /// 组件ID
    /// </summary>
    public string Plugin => plugin;
}