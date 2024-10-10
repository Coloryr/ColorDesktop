using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorDesktop.Api;

namespace ColorDesktop.CoreLib;

public static class CoreHelper
{
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
