using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

public interface IInstance
{
    /// <summary>
    /// 实例启动
    /// </summary>
    void Start(IInstanceWindow window);
    /// <summary>
    /// 实例停止
    /// </summary>
    void Stop(IInstanceWindow window);

    /// <summary>
    /// 渲染
    /// </summary>
    void RenderTick();
    /// <summary>
    /// 创建显示图层
    /// </summary>
    /// <returns></returns>
    Control CreateView();
    /// <summary>
    /// 配置更新
    /// </summary>
    /// <param name="obj">实例设置</param>
    void Update(InstanceDataObj obj);
    /// <summary>
    /// 获取窗口控制
    /// </summary>
    /// <returns></returns>
    IInstanceHandel? GetHandel();
}
