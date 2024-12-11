using Avalonia.Controls;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.Api;

public interface IPlugin
{
    /// <summary>
    /// 是否可以控制开关
    /// 为false时不能从组件设置里面手动禁用启用
    /// </summary>
    public bool CanEnable { get; }
    /// <summary>
    /// 是否可以创建实例
    /// </summary>
    public bool CanCreateInstance { get; }
    /// <summary>
    /// 是否有组件设置
    /// </summary>
    public bool HavePluginSetting { get; }
    /// <summary>
    /// 是否有实例设置
    /// </summary>
    public bool HaveInstanceSetting { get; }

    /// <summary>
    /// 加载语言
    /// </summary>
    /// <param name="type">语言类型</param>
    void LoadLang(LanguageType type);

    /// <summary>
    /// 初始化显示实例，你可以在此阶段设置实例的独立配置文件
    /// </summary>
    /// <param name="obj">实例数据</param>
    /// <returns>组件实例</returns>
    IInstance MakeInstances(InstanceDataObj obj);
    /// <summary>
    /// 创建一个显示实例默认参数，默认显示方式，启动参数
    /// </summary>
    /// <returns></returns>
    InstanceDataObj CreateInstanceDefault();
    /// <summary>
    /// 获取图标
    /// </summary>
    /// <returns></returns>
    Stream? GetIcon();
    /// <summary>
    /// 打开实例设置
    /// </summary>
    /// <param name="instance">实例信息</param>
    /// <param name="isNew">是否为新建</param>
    Control? OpenSetting(InstanceDataObj instance, bool isNew);
    /// <summary>
    /// 打开组件设置
    /// </summary>
    Control OpenSetting();
    /// <summary>
    /// 请求控制权限
    /// </summary>
    /// <param name="key">请求的插件</param>
    /// <param name="permission">权限</param>
    /// <returns>true 允许其他插件控制</returns>
    bool Permissions(string key, string permission);

    /// <summary>
    /// 组件初始化，可以设置组件全体配置
    /// </summary>
    /// <param name="local">运行路径</param>
    /// <param name="instancelocal">实例跟目录</param>
    /// <returns></returns>
    void Init(string local, string instancelocal);
    /// <summary>
    /// 组件启用
    /// </summary>
    void Enable();
    /// <summary>
    /// 组件禁用
    /// </summary>
    void Disable();
    /// <summary>
    /// 组件停止
    /// </summary>
    void Stop();
}
