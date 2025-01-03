namespace ColorDesktop.Api.Events;

public abstract class GroupBaseEvent(string? group) : BaseEvent
{
    /// <summary>
    /// 分组UUID
    /// </summary>
    public string? Group => group;
}
