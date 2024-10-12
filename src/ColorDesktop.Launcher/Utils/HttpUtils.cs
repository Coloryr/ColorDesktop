using System.Net.Http;
using System.Net.Http.Headers;

namespace ColorDesktop.Launcher.Utils;

public static class HttpUtils
{
    public static readonly HttpClient Client;

    public const string Url = "https://www.coloryr.com/colordesktop/plugin.json";

    static HttpUtils()
    {
        Client = new();
        Client.DefaultRequestHeaders.UserAgent.Clear();
        Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ColorDesktop", "1.0.0"));
    }
}
