using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

/// <summary>
/// 实例窗口
/// </summary>
public interface IInstanceWindow
{
    /// <summary>
    /// 在最前显示
    /// </summary>
    void Activate();
    /// <summary>
    /// 显示
    /// </summary>
    void Show();
    /// <summary>
    /// 关闭
    /// </summary>
    void Close();
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Move(int x, int y);
    /// <summary>
    /// 设置窗口状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    void SetState(WindowState state);
    /// <summary>
    /// 设置窗口透明
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    void SetTran(WindowTransparencyType level);
}
