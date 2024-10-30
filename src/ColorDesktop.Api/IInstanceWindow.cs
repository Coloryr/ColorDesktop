namespace ColorDesktop.Api;

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
    /// 更新配置
    /// </summary>
    /// <param name="obj"></param>
    void Update(InstanceDataObj obj);
}
