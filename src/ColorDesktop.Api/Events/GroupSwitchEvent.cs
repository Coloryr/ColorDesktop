namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例分组切换事件
/// </summary>
/// <param name="group"></param>
public class GroupSwitchEvent(string? old, string? group) : GroupBaseEvent(group)
{
    /// <summary>
    /// 旧的实例分组UUID
    /// </summary>
    public string? OldGroup => old;
}
