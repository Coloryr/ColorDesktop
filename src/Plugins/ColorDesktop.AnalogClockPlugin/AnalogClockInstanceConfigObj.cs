namespace ColorDesktop.AnalogClockPlugin;

public enum ClockType
{
    Analog, Digital, Flip
}

public record AnalogClockInstanceConfigObj
{
    public ClockType Type { get; set; }
    public int Size { get; set; }
    public bool DisplaySecond { get; set; }
    public string Color { get; set; }
    public bool Blink { get; set; }
    public int TextSize { get; set; }
    public string TextColor { get; set; }
    public string BackColor { get; set; }
    public string BorderColor { get; set; }
    public bool UseFont { get; set; }
    public string Font { get; set; }
}
