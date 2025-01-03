﻿namespace ColorDesktop.Api.Events;

/// <summary>
/// 实例事件
/// </summary>
/// <param name="plugin"></param>
/// <param name="uuid"></param>
public abstract class InstanceEvent(string plugin, string? group, string uuid) : BaseEvent
{
    /// <summary>
    /// 组件ID
    /// </summary>
    public string Plugin => plugin;
    /// <summary>
    /// 实例UUID
    /// </summary>
    public string UUID => uuid;
    /// <summary>
    /// 实例分组
    /// </summary>
    public string? Group => group;
}

