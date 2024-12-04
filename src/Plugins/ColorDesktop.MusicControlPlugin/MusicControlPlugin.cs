using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;
using ColorDesktop.MusicControlPlugin.Hook;

namespace ColorDesktop.MusicControlPlugin;

public class MusicControlPlugin : IPlugin
{
    public const string ConfigName = "musiccontrol.json";

    public static IHook? MusicHook;

    public static MusicInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new MusicInstanceObj()
        {
            Width = 300
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, MusicInstanceObj config)
    {
        InstanceUtils.SaveConfig(obj, config, ConfigName);
    }

    public bool IsCoreLib => false;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => true;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        {
            Nick = LangApi.GetLang("MusicControlPlugin.Name"),
            Plugin = "coloryr.music.control",
            Pos = PosEnum.TopRight,
            Margin = new(5)
        };
    }

    public void Disable()
    {

    }

    public void Enable()
    {

    }

    public Stream? GetIcon()
    {
        var assm = Assembly.GetExecutingAssembly();
        var item = assm.GetManifestResourceStream("ColorDesktop.MusicControlPlugin.icon.png")!;
        return item;
    }

    public async void Init(string local, string local1)
    {
        switch (SystemInfo.Os)
        {
            case OsType.Windows:
                MusicHook = await Win32.Init();
                break;
            case OsType.Linux:
                MusicHook = await Linux.Init();
                break;
        }
    }

    public void LoadLang(LanguageType type)
    {
        var assm = Assembly.GetExecutingAssembly();
        if (assm == null)
        {
            return;
        }
        string name = type switch
        {
            LanguageType.en_us => "ColorDesktop.MusicControlPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.MusicControlPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new MusicControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new MusicInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {
        MusicHook?.Stop();
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
