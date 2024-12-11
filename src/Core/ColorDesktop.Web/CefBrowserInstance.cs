using Avalonia.Controls;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Newtonsoft.Json;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Events;
using Xilium.CefGlue.Common.Handlers;

namespace ColorDesktop.Web;

public class CefBrowserInstance(InstanceDataObj obj) : IInstance
{
    private AvaloniaCefBrowser _browser;
    private readonly WebInstanceObj _config = InstanceUtils.GetConfig(obj, new WebInstanceObj(), "webplugin.json");
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
        _browser.Address = "http://localhost:" + HttpWeb.Port + "/" + _config.Plugin;
        return _browser;
    }

    private void Browser_LoadEnd(object sender, LoadEndEventArgs e)
    {
        _ok = true;
        if (_reload)
        {
            _reload = false;
            _browser.ExecuteJavaScript($"colordesktop.start()");
        }
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
        if (!_ok)
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
        string jsonData = JsonConvert.SerializeObject(obj);
        _browser.ExecuteJavaScript($"colordesktop.update({jsonData})");
    }

    public void WindowLoaded(IInstanceWindow window)
    {
        
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
                avalonia._ok = false;
                avalonia._reload = true;
                avalonia._browser.Reload();
                return true;
            }
            return false;
        }
    }

    private class BrowserLifeSpanHandler(AvaloniaCefBrowser avalonia) : LifeSpanHandler
    {
        protected override bool OnBeforePopup(
            CefBrowser browser,
            CefFrame frame,
            string targetUrl,
            string targetFrameName,
            CefWindowOpenDisposition targetDisposition,
            bool userGesture,
            CefPopupFeatures popupFeatures,
            CefWindowInfo windowInfo,
            ref CefClient client,
            CefBrowserSettings settings,
            ref CefDictionaryValue extraInfo,
            ref bool noJavascriptAccess)
        {
            var bounds = avalonia.Bounds;
            Dispatcher.UIThread.Post(() =>
            {
                var window = new Window();
                var popupBrowser = new AvaloniaCefBrowser
                {
                    Address = targetUrl
                };
                window.Content = popupBrowser;
                window.Height = bounds.Height;
                window.Width = bounds.Width;
                window.Title = targetUrl;
                window.Show();
            });
            return true;
        }
    }
}
