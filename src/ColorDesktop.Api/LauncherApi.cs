using System.Collections.Concurrent;
using ColorDesktop.Api.Events;

namespace ColorDesktop.Api;

public static class LauncherApi
{
    /// <summary>
    /// API版本
    /// </summary>
    public const string ApiVersion = "3";

    /// <summary>
    /// 启动器接口
    /// </summary>
    public static ILauncherHook Hook { get; private set; }

    private static readonly ConcurrentDictionary<string, object?> s_shareData = [];
    private static readonly ConcurrentDictionary<string, List<Action<BaseEvent>>> s_pluginEvent;

    /// <summary>
    /// 公共数据获取，只能存基础类型
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="obj">值</param>
    /// <returns>是否有数据</returns>
    public static bool GetData(string key, out object? obj)
    {
        return s_shareData.TryGetValue(key, out obj);
    }
    /// <summary>
    /// 公共数据设置，只能存基础类型
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="obj">值</param>
    public static void SetData(string key, object? obj)
    {
        if (!s_shareData.TryAdd(key, obj))
        {
            s_shareData[key] = obj;
        }
    }
    /// <summary>
    /// 公共数据删除
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>是否成功删除</returns>
    public static bool RemoveData(string key)
    {
        return s_shareData.TryRemove(key, out _);
    }
    /// <summary>
    /// 监听事件
    /// </summary>
    public static void AddListener(IPlugin plugin, Action<BaseEvent> action)
    {
        var id = Hook.GetPluginId(plugin);
        if (id == null)
        {
            return;
        }
        if (s_pluginEvent.TryGetValue(id, out var list))
        {
            list.Add(action);
            return;
        }
        s_pluginEvent.TryAdd(id, [action]);
    }
    /// <summary>
    /// 取消所有监听
    /// </summary>
    /// <param name="id"></param>
    public static void RemoveListener(string id)
    {
        s_pluginEvent.Remove(id, out _);
    }

    /// <summary>
    /// 触发组件事件
    /// </summary>
    /// <param name="pluginEvent"></param>
    public static void CallEvent(BaseEvent pluginEvent)
    {
        foreach (var item in s_pluginEvent)
        {
            try
            {
                item.Value.ForEach(item1 => item1.Invoke(pluginEvent));
            }
            catch (Exception e)
            {
                Logs.Error("event call crash", e);
            }
        }
    }

    public static void Init(ILauncherHook hook)
    {
        Hook ??= hook;
    }
}
