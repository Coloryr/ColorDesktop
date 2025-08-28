namespace ColorDesktop.TimerPlugin;

public enum TimerType
{
    Down, Up
}

public record TimerItemObj
{
    public string Name { get; set; }
    public TimerType Type { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan Interval { get; set; }
}

public record TimerInstanceObj
{
    public int MaxHeight { get; set; }
    public List<TimerItemObj> Timers { get; set; }
}
