using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace ColorDesktop.Api;

/// <summary>
/// 组件/实例 设置接口
/// </summary>
public class InstanceSetting
{
    /// <summary>
    /// 需要显示的控件
    /// </summary>
    public Control? Control;
    /// <summary>
    /// 完成设置回调
    /// </summary>
    public Action? Close;
}
