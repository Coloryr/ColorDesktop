using System.Text.Json.Nodes;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace ColorDesktop.Api;

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
        var value = LangApi.GetLang(key);
        observer.OnNext(value);
        return new UnsubscribeLang(list, observer);
    }

    public static void Reload()
    {
        foreach (var item in s_langList)
        {
            var value = LangApi.GetLang(item.Key);
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

public static class LangApi
{
    private static readonly Language s_language = new();

    public static void AddLangs(string data)
    {
        s_language.Load(data);
    }

    public static void ClearLangs()
    {

    }

    public static string GetLang(string key)
    {
        return s_language.GetLanguage(key);
    }
}

/// <summary>
/// 语言储存
/// </summary>
public class Language
{
    private readonly Dictionary<string, string> _languageList = [];

    /// <summary>
    /// 加载语言
    /// </summary>
    /// <param name="item"></param>
    public void Load(string item)
    {
        if (JsonUtils.ReadAsObj(item) is JsonObject json)
        {
            foreach (var item1 in json)
            {
                _languageList.Remove(item1.Key);
                _languageList.TryAdd(item1.Key, item1.Value!.AsValue().GetValue<string>());
            }
        }
    }

    /// <summary>
    /// 获取语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetLanguage(string key)
    {
        if (_languageList.TryGetValue(key, out var res1))
        {
            return res1;
        }

        return key;
    }

    /// <summary>
    /// 获取语言
    /// </summary>
    /// <param name="key"></param>
    /// <param name="have"></param>
    /// <returns></returns>
    public string GetLanguage(string key, out bool have)
    {
        if (_languageList.TryGetValue(key, out var res1))
        {
            have = true;
            return res1!;
        }

        have = false;
        return key;
    }
}

public class LocalizeExtension(string key) : MarkupExtension, IObservable<string>
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this.ToBinding();
    }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        return LangSel.Add(key, observer);
    }
}

public class UnsubscribeLang(List<WeakReference<IObserver<string>>> observers, IObserver<string> observer) : IDisposable
{
    public void Dispose()
    {
        foreach (var item in observers.ToArray())
        {
            if (!item.TryGetTarget(out var target)
                || target == observer)
            {
                observers.Remove(item);
            }
        }
    }
}