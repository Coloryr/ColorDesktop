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
    /// 水平对齐方式
    /// </summary>
    public HorizontalAlignment Horizontal { get; set; }
    /// <summary>
    /// 垂直对齐方式
    /// </summary>
    public VerticalAlignment Vertical { get; set; }
    /// <summary>
    /// 间距
    /// </summary>
    public Thickness Margin { get; set; }
    /// <summary>
    /// 是否为自定义窗口
    /// </summary>
    public bool IsWindow { get; set; }
    /// <summary>
    /// 自定义传递的参数，这部分会被固化，请时使用能够Json处理的数据格式
    /// </summary>
    public JObject Arg { get; set; }
}
