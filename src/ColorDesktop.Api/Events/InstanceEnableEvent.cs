namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例启用
/// </summary>
/// <param name="plugin"></param>
/// <param name="uuid"></param>
public class InstanceEnableEvent(string plugin, string? group, string uuid) : InstanceEvent(plugin, group, uuid)
{
}
