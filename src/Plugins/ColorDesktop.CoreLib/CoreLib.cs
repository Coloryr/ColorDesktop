using System.Reflection;
using Avalonia.Controls;
using ColorDesktop.Api;

namespace ColorDesktop.CoreLib;

public class CoreLib : IPlugin
{
    public static string Local;
    public static string InstanceLocal;

    public bool IsCoreLib => true;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => false;

    public InstanceDataObj CreateInstanceDefault()
    {
        throw new NotImplementedException();
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
        var item = assm.GetManifestResourceStream("ColorDesktop.CoreLib.icon.png")!;
        return item;
    }

    public void Init(string local, string local1)
    {
        Local = local;
        InstanceLocal = local1;

        TempManager.Init(local);
    }

    public void LoadLang(LanguageType type)
    {

    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        return new();
    }

    public Control OpenSetting()
    {
        return new();
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }

    public void Stop()
    {

    }
}
