using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Newtonsoft.Json;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;

namespace ColorDesktop.Web;

public class CefBrowserInstance(InstanceDataObj obj) : IInstance
{
    private AvaloniaCefBrowser browser;

    public Control CreateView()
    {
        browser = new();
        browser.KeyDown += Browser_KeyDown;
        browser.LifeSpanHandler = new BrowserLifeSpanHandler();
        browser.Address = "http://localhost:" + HttpWeb.Port + "/" + obj.Plugin + "/index.html";
        return browser;
    }

    public IInstanceHandel? GetHandel()
    {
        var res = browser.EvaluateJavaScript<bool>($"colordesktop_ishandel()").Result;
        if (res)
        {
            return new WebInstanceHandel(browser);
        }
        return null;
    }

    public void RenderTick()
    {
        browser.ExecuteJavaScript($"colordesktop_render()");
    }

    public void Start(IInstanceWindow window)
    {
        browser.RegisterJavascriptObject(new JsHandel(window), "colordesktop_window");
        browser.ExecuteJavaScript($"colordesktop_start()");
    }

    private void Browser_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.F12)
        {
            browser.ShowDeveloperTools();
        }
    }

    public void Stop(IInstanceWindow window)
    {
        browser.ExecuteJavaScript($"colordesktop_stop()");
        browser.UnregisterJavascriptObject("colordesktop");
        browser.Dispose();
    }

    public void Update(InstanceDataObj obj)
    {
        string jsonData = JsonConvert.SerializeObject(obj);
        browser.ExecuteJavaScript($"colordesktop_update({jsonData})");
    }

    private class WebInstanceHandel(AvaloniaCefBrowser browser) : IInstanceHandel
    {
        public ManagerState Move(int x, int y)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop_move({x}, {y})").Result;
        }

        public ManagerState Resize(int x, int y)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop_resize({x}, {y})").Result;
        }

        public ManagerState SetState(WindowState state)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop_setstate({state})").Result;
        }

        public ManagerState SetTran(WindowTransparencyType level)
        {
            return browser.EvaluateJavaScript<ManagerState>($"colordesktop_move({level})").Result;
        }
    }

    private class BrowserLifeSpanHandler : LifeSpanHandler
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
            var bounds = windowInfo.Bounds;
            Dispatcher.UIThread.Post(() =>
            {
                var window = new Window();
                var popupBrowser = new AvaloniaCefBrowser
                {
                    Address = targetUrl
                };
                window.Content = popupBrowser;
                window.Position = new PixelPoint(bounds.X, bounds.Y);
                window.Height = bounds.Height;
                window.Width = bounds.Width;
                window.Title = targetUrl;
                window.Show();
            });
            return true;
        }
    }
}
