using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ColorDesktop.Api;

public record MarginObj
{
    public MarginObj()
    { 
        
    }

    public MarginObj(int right, int top, int left, int bottom)
    {
        Right = right;
        Top = top;
        Left = left;
        Bottom = bottom;
    }
    public MarginObj(int a) : this(a, a, a, a) { }

    public int Right { get; set; }
    public int Top { get; set; }
    public int Left { get; set; }
    public int Bottom { get; set; }
}

public sealed record InstanceDataObj
{
    /// <summary>
    /// 不要设置
    /// </summary>
    public string UUID { get; set; }
    /// <summary>
    /// 默认实例的名字
    /// </summary>
    public string Nick { get; set; }
    /// <summary>
    /// 插件ID
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
    /// 在那个显示器
    /// </summary>
    public int Display { get; set; }
    /// <summary>
    /// 是否为自定义窗口
    /// </summary>
    public bool IsWindow { get; set; }
    /// <summary>
    /// 自定义传递的参数，这部分会被固化，请时使用能够Json处理的数据格式
    /// 这部分不要用来做配置存储，只用于传递启动参数
    /// </summary>
    public JObject Arg { get; set; }
}
