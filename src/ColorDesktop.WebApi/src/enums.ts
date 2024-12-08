export enum ManagerState {
    /**
     * 组件未找到
     */
    PluginNotFound,
    /**
     * 实例未找到
     */
    InstanceNotFound,
    /**
     * 没有请求过权限
     */
    NoTestPermission,
    /**
     * 没有权限
     */
    NoPermission,
    /**
     * 组件已经启用了
     */
    PluginIsEnabled,
    /**
     * 实例已经启用了
     */
    InstanceIsEnabled,
    /**
     * 组件已经禁用了
     */
    PluginIsDisabled,
    /**
     * 实例已经禁用了
     */
    InstanceIsDisabled,
    /**
     * 操作成功
     */
    Success,
    /**
     * 操作失败
     */
    Fail
}

export enum WindowState {
    Normal = 0,
    Minimized = 1,
    Maximized = 2,
    FullScreen = 3
}

export enum WindowTransparencyType {
    None, Transparent, Blur, AcrylicBlur, Mica
}

export enum PosEnum {
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