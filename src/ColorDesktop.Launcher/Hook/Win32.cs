using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Microsoft.Win32;

namespace ColorDesktop.Launcher.Hook;

public static class Win32
{
    public static void SetLaunch(bool add)
    {
#pragma warning disable CA1416 // 验证平台兼容性
        var registryKey = Registry.CurrentUser.OpenSubKey
     ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (registryKey == null)
        {
            return;
        }
        if (add)
        {
            var executablePath = Environment.ProcessPath;
            if (executablePath == null)
            {
                return;
            }
            registryKey.SetValue("ColorDekstop", executablePath);
        }
        else
        {
            registryKey.DeleteValue("ColorDekstop");
        }
#pragma warning restore CA1416 // 验证平台兼容性
    }
}
