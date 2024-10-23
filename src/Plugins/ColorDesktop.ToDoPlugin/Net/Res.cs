using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.ToDoPlugin.Objs;

namespace ColorDesktop.ToDoPlugin.Net;

/// <summary>
/// 登录结果
/// </summary>
public enum LoginState
{
    /// <summary>
    /// 完成
    /// </summary>
    Done,
    /// <summary>
    /// 请求超时
    /// </summary>
    TimeOut,
    /// <summary>
    /// 数据错误
    /// </summary>
    DataError,
    /// <summary>
    /// 错误
    /// </summary>
    Error,
    /// <summary>
    /// 发送崩溃
    /// </summary>
    Crash
}

/// <summary>
/// OAuth获取登陆码结果
/// </summary>
public record OAuthGetCodeRes
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public LoginState State;
    /// <summary>
    /// 登录码
    /// </summary>
    public string? Code;
    /// <summary>
    /// 消息
    /// </summary>
    public string? Message;
}

/// <summary>
/// OAuth获取登录返回结果
/// </summary>
public record OAuthGetTokenRes
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public LoginState State;
    /// <summary>
    /// 信息
    /// </summary>
    public OAuthGetCodeObj? Obj;
}