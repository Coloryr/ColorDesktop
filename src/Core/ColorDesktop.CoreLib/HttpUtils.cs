using System.Net.Http.Headers;

namespace ColorDesktop.CoreLib;

public static class HttpUtils
{
    public static readonly HttpClient Client;

    static HttpUtils()
    {
        Client = new();
        Client.DefaultRequestHeaders.UserAgent.Clear();
        Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ColorDesktop", "1.0.0"));
    }
}
