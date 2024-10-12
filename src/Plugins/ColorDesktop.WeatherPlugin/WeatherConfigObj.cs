namespace ColorDesktop.WeatherPlugin;

public record WeatherConfigObj
{
    public bool AutoUpdate { get; set; }
    public int UpdateTime { get; set; }
}
