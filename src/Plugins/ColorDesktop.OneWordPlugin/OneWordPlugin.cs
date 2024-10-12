using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.OneWordPlugin;

public class OneWordPlugin : IPlugin
{
    public const string ConfigName = "oneword.json";

    public static OneWordInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new OneWordInstanceObj()
        {
            Size = 80,
            TextColor = "#FFFFFF",
            BackColor = "#000000",
            Width = 600
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, OneWordInstanceObj config)
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
            Nick = "每日一言",
            Plugin = "coloryr.oneword",
            Pos = PosEnum.Top,
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
        var item = assm.GetManifestResourceStream("ColorDesktop.OneWordPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1, LanguageType type)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new OneWordControl();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new OneWordInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }
}
