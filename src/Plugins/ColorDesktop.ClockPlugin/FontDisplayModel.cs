using Avalonia.Media;

namespace ColorDesktop.ClockPlugin;

public record FontDisplayModel
{
    /// <summary>
    /// 字体名字
    /// </summary>
    public string FontName { get; init; }
    /// <summary>
    /// 字体样式
    /// </summary>
    public FontFamily FontFamily { get; init; }

    public override string ToString()
    {
        return FontName;
    }
}
