using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using ColorDesktop.CoreLib;

namespace ColorDesktop.ToDoPlugin;

public class ToDoPlugin : IPlugin
{
    public const string ConfigName = "todo.json";

    public static ToDoInstanceObj GetConfig(InstanceDataObj obj)
    {
        return obj.GetConfig(new ToDoInstanceObj()
        {
            Width = 300,
            Height = 500,
            BackColor = "#5d71bf",
            TextColor = "#000000"
        }, ConfigName, JsonGen.Default.ToDoInstanceObj);
    }

    public static void SaveConfig(InstanceDataObj obj, ToDoInstanceObj config)
    {
        obj.SaveConfig(config, ConfigName, JsonGen.Default.ToDoInstanceObj);
    }

    public bool CanCreateInstance => true;
    public bool HavePluginSetting => false;
    public bool HaveInstanceSetting => true;
    public bool CanEnable => true;

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

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        return new() { Control = new ToDoInstanceSettingControl(instance) };
    }

    public InstanceSetting OpenSetting()
    {
        return new();
    }

    public void Stop()
    {

    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }
}
