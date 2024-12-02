namespace ColorDesktop.Api;

public enum LanguageType
{
    zh_cn, en_us
}

public enum PosEnum
{
    TopLeft,
    Top,
    TopRight,
    Left,
    Center,
    Right,
    BottomLeft,
    Bottom,
    BottomRight
}

public enum InstanceState
{
    /// <summary>
    /// 加载错误
    /// </summary>
    LoadError,
    /// <summary>
    /// 加载失败
    /// </summary>
    LoadFail,
    PluginDisable,
    PluginNotFound,
    Disable,
    Enable
}

public enum PluginState
{
    /// <summary>
    /// 系统不支持
    /// </summary>
    OsError,
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