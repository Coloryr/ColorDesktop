using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace ColorDesktop.Api;

public interface IPlugin
{
    /// <summary>
    /// 初始化显示实例，你可以在此阶段设置实例的独立配置文件
    /// </summary>
    /// <param name="local">运行路径</param>
    /// <param name="arg">启动参数</param>
    /// <returns>组件实例</returns>
    IInstance MakeInstances(string local, InstanceDataObj arg);
    /// <summary>
    /// 创建一个显示实例默认参数，默认显示方式，启动参数
    /// </summary>
    /// <returns></returns>
    InstanceDataObj CreateInstanceDefault();
    /// <summary>
    /// 显示实例位置设置完成后，进行组件的某些内容设置
    /// 返回空则表示不需要额外设置
    /// </summary>
    /// <returns></returns>
    Window? CreateInstanceSetting(InstanceDataObj data);
    /// <summary>
    /// 获取图标
    /// </summary>
    /// <returns></returns>
    Bitmap? GetIcon();

    /// <summary>
    /// 插件初始化，可以设置插件全体配置
    /// </summary>
    /// <param name="local">运行路径</param>
    /// <param name="type">默认语言</param>
    /// <returns></returns>
    void Init(string local, LanguageType type);
    /// <summary>
    /// 插件启用
    /// </summary>
    void Enable();
    /// <summary>
    /// 插件禁用
    /// </summary>
    void Disable();
    /// <summary>
    /// 插件停止
    /// </summary>
    void Stop();
    /// <summary>
    /// 打开插件设置
    /// </summary>
    void OpenSetting();
    /// <summary>
    /// 打开实例设置
    /// </summary>
    /// <param name="instance"></param>
    void OpenSetting(InstanceDataObj instance);
}
