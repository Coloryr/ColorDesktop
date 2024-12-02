namespace ColorDesktop.Api;

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