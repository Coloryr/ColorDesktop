using Avalonia.Controls;
using Avalonia.Media.Imaging;
using ColorDesktop.Api;

namespace ColorDesktop.BmPlugin;

public class BmPlugin : IPlugin
{
    public bool IsCoreLib => false;

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

    public Bitmap? GetIcon()
    {
        return null;
    }

    public void Init(string local, string local1, LanguageType type)
    {
        
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting(InstanceDataObj instance)
    {
        throw new NotImplementedException();
    }

    public Control OpenSetting()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        
    }
}
