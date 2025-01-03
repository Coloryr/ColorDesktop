namespace ColorDesktop.Api.Events;

public class DataEvent : BaseEvent
{
    public object?[] Objects { get; set; }
}
