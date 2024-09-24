namespace ColorDesktop.ClockPlugin;

public record ClockInstanceObj
{
    public bool UseFont { get; set; }
    public string FontName { get; set; }
    public bool DisplaySecond { get; set; }
    public bool Blink { get; set; }
    public bool CustomColor { get; set; }
    public bool CustomSize { get; set; }
    public string Color { get; set; }
    public int Size { get; set; }
    public string HourColor { get; set; }
    public int HourSize { get; set; }
    public string MinuteColor { get; set; }
    public int MinuteSize { get; set; }
    public string SecondColor { get; set; }
    public int SecondSize { get; set; }
    public string ColonColor { get; set; }
    public int ColonSize { get; set; }
    public string BackGround { get; set; }
}
