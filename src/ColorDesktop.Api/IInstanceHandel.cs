using Avalonia.Controls;

namespace ColorDesktop.Api;

public interface IInstanceHandel
{
    /// <summary>
    /// 移动窗口
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public ManagerState Move(int x, int y);
    /// <summary>
    /// 重新设置窗口大小
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public ManagerState Resize(int x, int y);
    /// <summary>
    /// 设置窗口状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public ManagerState SetState(WindowState state);
    /// <summary>
    /// 设置窗口透明
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public ManagerState SetTran(WindowTransparencyType level);
}
