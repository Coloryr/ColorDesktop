namespace ColorDesktop.Launcher.Objs;

public enum InstanceState
{
    LoadError, LoadFail, PluginDisable, PluginNotFound, Disable, Enable
}

public enum PluginState
{
    LoadError, EnableError, DepNotFound, Disable, Enable
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