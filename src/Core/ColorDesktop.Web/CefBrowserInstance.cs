using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Events;
using Xilium.CefGlue.Common.Handlers;

namespace ColorDesktop.Web;

public class CefBrowserInstance(InstanceDataObj obj) : IInstance
{
    private AvaloniaCefBrowser _browser;
    private IInstanceWindow _window;

    private bool _ok = false;
    private bool _reload = false;

    public Control CreateView()
    {
        _browser = new()
        {
            Width = 300,
            Height = 300
        };
        _browser.LifeSpanHandler = new BrowserLifeSpanHandler(_browser);
        _browser.KeyboardHandler = new BrowserKeyboardHandler(this);
        _browser.LoadEnd += Browser_LoadEnd;
        _browser.Address = "http://localhost:" + HttpWeb.Port + "/" + obj.UUID;
        return _browser;
    }

    private async Task Browser_LoadEnd(object sender, LoadEndEventArgs e)
    {
        if (_reload)
        {
            _browser.RegisterJavascriptObject(new JsHandel(_browser, _window), "colordesktop_window");
            try
            {
                var res = await _browser.EvaluateJavaScript<bool>($"colordesktop.start()");
                _ok = true;
            }
            catch
            {
                _ok = false;
            }
            _reload = false;
        }
    }

    private void Reload()
    {
        _ok = false;
        _reload = true;
        _browser.UnregisterJavascriptObject("colordesktop_window");
        _browser.Reload();
    }

    public IInstanceHandel? GetHandel()
    {
        var res = _browser.EvaluateJavaScript<bool>($"colordesktop.haveHandel()").Result;
        if (res)
        {
            return new WebInstanceHandel(_browser);
        }
        return null;
    }

    public void RenderTick(IInstanceWindow window)
    {
        if (!_ok || _reload)
        {
            return;
        }
        _browser.ExecuteJavaScript($"colordesktop.render()");
    }

    public void Start(IInstanceWindow window)
    {
        _browser.RegisterJavascriptObject(new JsHandel(_browser, window), "colordesktop_window");
        if (!_ok)
        {
            Task.Run(async () =>
            {
                while (!_ok)
                {
                    await Task.Delay(200);
                }

                _browser.ExecuteJavaScript($"colordesktop.setWindow(colordesktop_window)");
                _browser.ExecuteJavaScript($"colordesktop.start()");
            });
        }
    }

    public void Stop(IInstanceWindow window)
    {
        _browser.ExecuteJavaScript($"colordesktop.stop()");
        _browser.UnregisterJavascriptObject("colordesktop_window");
        _browser.Dispose();
    }

    public void Update(InstanceDataObj obj)
    {
        string jsonData = JsonSerializer.Serialize(obj, JsonGen.Default.InstanceDataObj);
        _browser.ExecuteJavaScript($"colordesktop.update({jsonData})");
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        _window = window;
    }

    public void OpenSetting()
    {
        _browser.ExecuteJavaScript("colordesktop.showSetting()");
    }

    public void CloseSetting()
    {
        _browser.ExecuteJavaScript("colordesktop.closeSetting()");
    }

    private class WebInstanceHandel(AvaloniaCefBrowser browser) : IInstanceHandel
    {
        public ManagerState Move(int x, int y)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop.move({x}, {y})").Result;
        }

        public ManagerState Resize(int x, int y)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop.resize({x}, {y})").Result;
        }

        public ManagerState SetState(WindowState state)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop.setState({(int)state})").Result;
        }

        public ManagerState SetTran(WindowTransparencyType level)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop.setTran({(int)level})").Result;
        }
    }

    private class BrowserKeyboardHandler(CefBrowserInstance avalonia) : KeyboardHandler
    {
        protected override bool OnKeyEvent(CefBrowser browser, CefKeyEvent keyEvent, nint osEvent)
        {
            if (keyEvent.WindowsKeyCode == 123)
            {
                avalonia._browser.ShowDeveloperTools();
                return true;
            }
            else if (keyEvent.WindowsKeyCode == 116)
            {
                avalonia.Reload();
                return true;
            }
            return false;
        }
    }
}
