using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;

namespace WebServerPluginDemo.Server;

public class WebServerPlugin : IPlugin
{
    public bool CanCreateInstance => throw new NotImplementedException();

    public bool HavePluginSetting => throw new NotImplementedException();

    public bool HaveInstanceSetting => throw new NotImplementedException();

    public bool CanEnable => throw new NotImplementedException();

    public InstanceDataObj CreateInstanceDefault()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public Stream? GetIcon()
    {
        throw new NotImplementedException();
    }

    public void Init(string local, string instancelocal)
    {
        throw new NotImplementedException();
    }

    public void LoadLang(LanguageType type)
    {
        throw new NotImplementedException();
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public InstanceSetting OpenSetting(InstanceDataObj instance, bool isNew)
    {
        throw new NotImplementedException();
    }

    public InstanceSetting OpenSetting()
    {
        throw new NotImplementedException();
    }

    public bool Permissions(string key, string permission)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
