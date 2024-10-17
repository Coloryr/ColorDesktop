using ColorDesktop.Api;

namespace ColorDesktop.MonitorPlugin;

public enum MonitorDisplayType
{
    Text, ProgressBar1, ProgressBar2
}

public enum PanelType
{
    Stack, Wrap, Panel
}

public enum ValueType
{
    Now, Max, Min
}

public record MonitorItemObj
{
    public MonitorDisplayType Display { get; set; }
    public ValueType ValueType { get; set; }
    public string Name { get; set; }
    public MarginObj Margin { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    /// <summary>
    /// 传感器ID
    /// </summary>
    public string Sensor { get; set; }

    /// <summary>
    /// 字体大小
    /// </summary>
    public int FontSize { get; set; }
    /// <summary>
    /// 边距
    /// </summary>
    public int BorderSize { get; set; }
    /// <summary>
    /// 格式化样式
    /// </summary>
    public string Format { get; set; }
    /// <summary>
    /// 最小数值
    /// </summary>
    public float Min { get; set; }
    /// <summary>
    /// 最大数值
    /// </summary>
    public float Max { get; set; }
    /// <summary>
    /// 背景色
    /// </summary>
    public string Color1 { get; set; }
    /// <summary>
    /// 字体颜色
    /// </summary>
    public string Color2 { get; set; }
}

public record MonitorInstanceObj
{
    public int Width { get; set; }
    public int Height { get; set; }
    public bool AutoSize { get; set; }
    public PanelType PanelType { get; set; }
    public List<MonitorItemObj> Items { get; set; }
}
