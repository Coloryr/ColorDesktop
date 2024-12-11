using System.Reflection;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace ColorDesktop.CoreLib;

public class CoreLib : IPlugin
{
    public static string Local;
    public static string InstanceLocal;

    public bool CanEnable => false;
    public bool CanCreateInstance => false;
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

    public InstanceSetting OpenSetting()
    {
        throw new NotImplementedException();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        throw new NotImplementedException();
    }

    public bool Permissions(string key, string permission)
    {
        return false;
    }

    public void Stop()
    {

    }
}
