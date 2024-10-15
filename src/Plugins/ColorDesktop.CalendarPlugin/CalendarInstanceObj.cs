namespace ColorDesktop.CalendarPlugin;

public enum WeekStart
{
    DaySun,
    DaySat,
    DayOne
}

public enum SkinType
{ 
    Skin1, Skin2
}

public record CalendarInstanceObj
{
    public string BackColor { get; set; }
    public string TextColor { get; set; }
    public WeekStart WeekStart { get; set; }
    public SkinType Skin { get; set; }
}
