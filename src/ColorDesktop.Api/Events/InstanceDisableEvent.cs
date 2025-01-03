namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例禁用
/// </summary>
/// <param name="plugin"></param>
/// <param name="uuid"></param>
public class InstanceDisableEvent(string plugin, string? group, string uuid) : InstanceEvent(plugin, group, uuid)
{

}
