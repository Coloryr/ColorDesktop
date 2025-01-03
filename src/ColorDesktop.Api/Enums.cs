namespace ColorDesktop.Api;

/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType
{
    /// <summary>
    /// 简体中文
    /// </summary>
    zh_cn,
    /// <summary>
    /// 英语
    /// </summary>
    en_us
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
    /// API版本不一样
    /// </summary>
    ApiError,
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

public enum ManagerState
{
    /// <summary>
    /// 组件未找到
    /// </summary>
    PluginNotFound,
    /// <summary>
    /// 实例未找到
    /// </summary>
    InstanceNotFound,
    /// <summary>
    /// 没有请求过权限
    /// </summary>
    NoTestPermission,
    /// <summary>
    /// 没有权限
    /// </summary>
    NoPermission,
    /// <summary>
    /// 组件已经启用了
    /// </summary>
    PluginIsEnabled,
    /// <summary>
    /// 实例已经启用了
    /// </summary>
    InstanceIsEnabled,
    /// <summary>
    /// 组件已经禁用了
    /// </summary>
    PluginIsDisabled,
    /// <summary>
    /// 实例已经禁用了
    /// </summary>
    InstanceIsDisabled,
    /// <summary>
    /// 操作成功
    /// </summary>
    Success,
    /// <summary>
    /// 操作失败
    /// </summary>
    Fail
}

/// <summary>
/// 实例分组操作
/// </summary>
public enum GroupEditType
{
    /// <summary>
    /// 添加
    /// </summary>
    Add,
    /// <summary>
    /// 删除
    /// </summary>
    Remove,
    /// <summary>
    /// 启用
    /// </summary>
    Enable,
    /// <summary>
    /// 禁用
    /// </summary>
    Disable
}