using ColorDesktop.Api;

namespace ColorDesktop.Web;

public class JsHandel(IInstanceWindow instance)
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
}
