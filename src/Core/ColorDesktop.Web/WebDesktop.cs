using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;

namespace ColorDesktop.Web;

public class WebDesktop : IPlugin
{
    private string cachePath;

    public const string ApiVersion = "1";

    public bool IsCoreLib => true;

    public bool HavePluginSetting => false;

    public bool HaveInstanceSetting => false;

    public InstanceDataObj CreateInstanceDefault()
    {
        return new InstanceDataObj()
        { 
            Nick = "Web",
            Plugin = "coloryr.web",
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
        return null;
    }

    public void Init(string local, string instance)
    {
        cachePath = Path.Combine(local, "CefGlue");
        CefRuntime.Load(local);
        CefRuntimeLoader.Initialize(new CefSettings()
        {
            RootCachePath = cachePath,
#if WINDOWLESS
            // its recommended to leave this off (false), since its less performant and can cause more issues
            WindowlessRenderingEnabled = true
#else
            WindowlessRenderingEnabled = false
#endif
        });
        PluginManager.Init(local);
        HttpWeb.Start();
    }

    public void LoadLang(LanguageType type)
    {
        
    }

    public IInstance MakeInstances(InstanceDataObj obj)
    {
        return new CefBrowserInstance(obj);
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
        CefRuntime.Shutdown(); // must shutdown cef to free cache files (so that cleanup is able to delete files)
    }
}
