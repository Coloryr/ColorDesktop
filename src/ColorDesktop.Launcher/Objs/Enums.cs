namespace ColorDesktop.Launcher.Objs;

public enum InstanceState
{
    LoadError, LoadFail, PluginDisable, PluginNotFound, Disable, Enable
}

public enum PluginState
{
    /// <summary>
    /// 加载错误
    /// </summary>
    LoadError, 
    /// <summary>
    /// 启用错误
    /// </summary>
    EnableError, 
    /// <summary>
    /// 缺少前置
    /// </summary>
    DepNotFound,
    /// <summary>
    /// 禁用
    /// </summary>
    Disable, 
    /// <summary>
    /// 启用
    /// </summary>
    Enable, 
    /// <summary>
    /// 未加载
    /// </summary>
    Unload
}

/// <summary>
/// 运行类型
/// </summary>
public enum RunType
{
    /// <summary>
    /// 程序
    /// </summary>
    Program,
    /// <summary>
    /// 预览器
    /// </summary>
    AppBuilder,
    /// <summary>
    /// 手机
    /// </summary>
    Phone
}