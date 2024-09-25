using Avalonia.Controls;
using Avalonia.Media.Imaging;
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

    public Bitmap? GetIcon()
    {
        return null;
    }

    public void Init(string local, string local1, LanguageType type)
    {
        Local = local;
        InstanceLocal = local1;
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
