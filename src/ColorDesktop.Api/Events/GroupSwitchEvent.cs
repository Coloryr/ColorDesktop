namespace ColorDesktop.Api.Events;

/// <summary>
/// 分组切换事件
/// </summary>
/// <param name="group"></param>
public class GroupSwitchEvent(string? group) : GroupBaseEvent(group)
{

}
