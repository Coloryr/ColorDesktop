namespace ColorDesktop.Api.Events;

/// <summary>
/// 分组删除事件
/// </summary>
/// <param name="group"></param>
public class GroupDeleteEvent(string? group) : GroupBaseEvent(group)
{
}
