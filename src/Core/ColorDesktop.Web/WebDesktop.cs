using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;

namespace ColorDesktop.Web;

public class WebDesktop : IPlugin
{
    private string cachePath;

    public static string InstanceLocal;

    public const string ApiVersion = LauncherApi.ApiVersion;

    public bool CanEnable => false;
    public bool CanCreateInstance => true;
    public bool HavePluginSetting => true;
    public bool HaveInstanceSetting => true;

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
        InstanceLocal = instance;
        cachePath = Path.Combine(local, "CefGlue");
        CefRuntimeLoader.Initialize(new CefSettings()
        {
            RootCachePath = cachePath,
            BrowserSubprocessPath = local,
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
        HttpWeb.Stop();
        CefRuntime.Shutdown();
    }

    public Control? OpenSetting(InstanceDataObj instance, bool isNew)
    {
        if (isNew)
        {
            return new SelectPluginControl(instance);
        }

        return null;
    }
}
