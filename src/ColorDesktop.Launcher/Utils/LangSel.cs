using System;
using System.Collections.Generic;

namespace ColorDesktop.Launcher.Utils;

/// <summary>
/// 文本获取 
/// </summary>
public static class LangSel
{
    private static readonly Dictionary<string, List<WeakReference<IObserver<string>>>> s_langList = [];

    public static IDisposable Add(string key, IObserver<string> observer)
    {
        if (s_langList.TryGetValue(key, out var list))
        {
            list.Add(new(observer));
        }
        else
        {
            list = [new(observer)];
            s_langList.Add(key, list);
        }
        var value = App.Lang(key);
        observer.OnNext(value);
        return new UnsubscribeLang(list, observer);
    }

    public static void Reload()
    {
        foreach (var item in s_langList)
        {
            var value = App.Lang(item.Key);
            foreach (var item1 in item.Value)
            {
                if (item1.TryGetTarget(out var target))
                {
                    target.OnNext(value);
                }
            }
        }
    }

    public static void Remove()
    {
        foreach (var item in s_langList.Values)
        {
            foreach (var item1 in item.ToArray())
            {
                if (!item1.TryGetTarget(out _))
                {
                    item.Remove(item1);
                }
            }
        }
    }
}