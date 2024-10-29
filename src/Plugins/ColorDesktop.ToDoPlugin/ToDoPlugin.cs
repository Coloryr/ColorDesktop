using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.CoreLib;

namespace ColorDesktop.ToDoPlugin;

public class ToDoPlugin : IPlugin
{
    public const string ConfigName = "todo.json";

    public static ToDoInstanceObj GetConfig(InstanceDataObj obj)
    {
        return InstanceUtils.GetConfig(obj, new ToDoInstanceObj()
        {
            Width = 300,
            Height = 500,
            BackColor = "#5d71bf",
            TextColor = "#000000"
        }, ConfigName);
    }

    public static void SaveConfig(InstanceDataObj obj, ToDoInstanceObj config)
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
            Nick = LangApi.GetLang("ToDoPlugin.Name"),
            Plugin = "coloryr.todo",
            Pos = PosEnum.TopLeft,
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
        var item = assm.GetManifestResourceStream("ColorDesktop.ToDoPlugin.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {

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
            LanguageType.en_us => "ColorDesktop.ToDoPlugin.Resource.Lang.en-us.json",
            _ => "ColorDesktop.ToDoPlugin.Resource.Lang.zh-cn.json"
        };
        using var item = assm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(item);
        LangApi.AddLangs(reader.ReadToEnd());
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new ToDoControl(obj.UUID);
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new ToDoInstanceSettingControl(instance);
    }

    public Control OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }
}
