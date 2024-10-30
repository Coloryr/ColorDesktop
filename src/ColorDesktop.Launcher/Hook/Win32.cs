using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
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

    public const uint WS_EX_TOOLWINDOW = 0x00000080;
    public const uint WS_VISIBLE = 0x10000000;
    public const uint WS_EX_APPWINDOW = 0x00040000;

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongW", ExactSpelling = true)]
    private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtrW", ExactSpelling = true)]
    private static extern IntPtr SetWindowLong64b(IntPtr hWnd, int nIndex, IntPtr value);

    public static uint SetWindowLong(IntPtr hWnd, int nIndex, uint value)
    {
        if (IntPtr.Size == 4)
        {
            return SetWindowLong32b(hWnd, nIndex, value);
        }
        else
        {
            return (uint)SetWindowLong64b(hWnd, nIndex, new IntPtr(value)).ToInt32();
        }
    }

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtrW", ExactSpelling = true)]
    public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongW", ExactSpelling = true)]
    public static extern uint GetWindowLong32b(IntPtr hWnd, int nIndex);

    public static uint GetWindowLong(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 4)
        {
            return GetWindowLong32b(hWnd, nIndex);
        }
        else
        {
            return GetWindowLongPtr(hWnd, nIndex);
        }
    }

    public enum WindowLongParam
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21
    }

    public static void SetTabGone(Window window)
    {
        if (window.TryGetPlatformHandle() is { } platformHandle)
        {
            var hwnd = platformHandle.Handle;
            var temp = GetWindowLong(hwnd, (int)WindowLongParam.GWL_EXSTYLE);

            temp &= ~WS_VISIBLE;
            temp |= WS_EX_TOOLWINDOW;
            temp &= ~WS_EX_APPWINDOW;

            SetWindowLong(hwnd, (int)WindowLongParam.GWL_EXSTYLE, temp);
        }
    }
}
