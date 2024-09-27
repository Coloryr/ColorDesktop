using Avalonia.Controls;

namespace ColorDesktop.Api;

public record MarginObj
{
    public MarginObj()
    {

    }

    public MarginObj(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
    public MarginObj(int a) : this(a, a, a, a) { }

    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
}

public enum WindowTransparencyType
{ 
    None, Transparent, Blur, AcrylicBlur, Mica
}

public sealed record InstanceDataObj
{
    /// <summary>
    /// 不要修改这个
    /// </summary>
    public string UUID { get; set; }
    /// <summary>
    /// 默认实例的名字，只能设置一次
    /// </summary>
    public string Nick { get; set; }
    /// <summary>
    /// 组件ID
    /// </summary>
    public required string Plugin { get; set; }
    /// <summary>
    /// 对齐方式
    /// </summary>
    public PosEnum Pos { get; set; }
    /// <summary>
    /// 间距
    /// </summary>
    public MarginObj Margin { get; set; }
    /// <summary>
    /// 窗口透明
    /// </summary>
    public WindowTransparencyType Tran { get; set; } = WindowTransparencyType.Transparent;
    /// <summary>
    /// 在那个显示器
    /// </summary>
    public int Display { get; set; }
    /// <summary>
    /// 是否为自定义窗口
    /// </summary>
    public bool IsWindow { get; set; }
    /// <summary>
    /// 是否在最前面显示
    /// </summary>
    public bool TopModel { get; set; }
}
