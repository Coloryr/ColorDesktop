namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例创建
/// </summary>
/// <param name="plugin"></param>
/// <param name="uuid"></param>
public class InstanceCreateEvent(string plugin, string? group, string uuid) : InstanceEvent(plugin, group, uuid)
{
}
