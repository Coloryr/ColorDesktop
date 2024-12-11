using Avalonia.Controls;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Events;

namespace ColorDesktop.Web;

public class CefInstanceSetting(InstanceDataObj obj)
{
    private AvaloniaCefBrowser _browser;
    private readonly WebInstanceObj _config = InstanceManager.GetConfig(obj, new WebInstanceObj(), "webplugin.json");
    private bool _ok = false;

    public Control CreateView()
    {
        _browser = new()
        {
            Width = 400,
            Height = 400
        };
        _browser.LifeSpanHandler = new BrowserLifeSpanHandler(_browser);
        _browser.LoadEnd += Browser_LoadEnd;
        _browser.Address = "http://localhost:" + HttpWeb.Port + "/" + _config.Plugin;
        return _browser;
    }

    private void Browser_LoadEnd(object sender, LoadEndEventArgs e)
    {
        if (!_ok)
        {
            _ok = true;
            _browser.ExecuteJavaScript($"colordesktop.showSetting()");
        }
    }

    public void Close()
    { 
        
    }
}
