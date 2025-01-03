namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例分组创建
/// </summary>
public class GroupAddEvent(string? group) : GroupBaseEvent(group)
{

}
