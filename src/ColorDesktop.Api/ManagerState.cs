using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorDesktop.Api;

public enum ManagerState
{ 
    /// <summary>
    /// 组件未找到
    /// </summary>
    PluginNotFound,
    /// <summary>
    /// 没有请求过权限
    /// </summary>
    NoTestPermission,
    /// <summary>
    /// 没有权限
    /// </summary>
    NoPermission, 
    /// <summary>
    /// 已经启用了
    /// </summary>
    IsEnabled,
    /// <summary>
    /// 已经禁用了
    /// </summary>
    IsDisabled,
    /// <summary>
    /// 操作成功
    /// </summary>
    Success
}