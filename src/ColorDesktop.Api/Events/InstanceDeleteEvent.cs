namespace ColorDesktop.Api.Events;

public class InstanceDeleteEvent(string plugin, string? group, string uuid) : InstanceEvent(plugin, group, uuid)
{

}
