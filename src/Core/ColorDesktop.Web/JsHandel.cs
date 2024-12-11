using Avalonia.Controls;
using ColorDesktop.Api;
using ColorDesktop.Api.Objs;
using Xilium.CefGlue.Avalonia;

namespace ColorDesktop.Web;

public class JsHandel(AvaloniaCefBrowser browser, IInstanceWindow instance)
{
    public void Activate()
    {
        instance.Activate();
    }

    public void Close()
    {
        instance.Close();
    }

    public void Show()
    {
        instance.Show();
    }

    public void Move(int x, int y)
    {
        instance.Move(x, y);
    }

    public void Resize(int x, int y)
    {
        browser.Width = x;
        browser.Height = y;
    }

    public void SetState(WindowState state)
    {
        instance.SetState(state);
    }

    public void SetTran(WindowTransparencyType level)
    {
        instance.SetTran(level);
    }
}
