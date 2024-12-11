using Avalonia.Controls;
using Avalonia.Threading;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;

namespace ColorDesktop.Web;

public class BrowserLifeSpanHandler(AvaloniaCefBrowser avalonia) : LifeSpanHandler
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