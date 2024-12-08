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
}
