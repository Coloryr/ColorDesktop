using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using ColorDesktop.Api;

namespace ColorDesktop.Launcher.Utils;

public static class LauncherUtils
{
    public static readonly HttpClient Client;

    public const string Url = "https://www.coloryr.com/colordesktop/plugin.json";

    static LauncherUtils()
    {
        Client = new();
        Client.DefaultRequestHeaders.UserAgent.Clear();
        Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ColorDesktop", "1.0.0"));
    }

    /// <summary>
    /// 在浏览器打开网址
    /// </summary>
    /// <param name="url">网址</param>
    public static void OpUrl(string? url)
    {
        url = url?.Replace(" ", "%20");
        switch (SystemInfo.Os)
        {
            case OsType.Windows:
                var ps = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd",
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                });
                if (ps != null)
                {
                    ps.StandardInput.WriteLine($"start {url}");
                    ps.Close();
                }
                break;
            case OsType.Linux:
                Process.Start("xdg-open",
                    '"' + url + '"');
                break;
            case OsType.MacOS:
                Process.Start("open", "-a Safari " +
                    '"' + url + '"');
                break;
        }
    }
}
