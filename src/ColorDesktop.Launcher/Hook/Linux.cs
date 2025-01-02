using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace ColorDesktop.Launcher.Hook;

public static partial class Linux
{
    public enum Shape
    {
        Bounding = 0,
        Clip = 1,
        Input = 2
    }

    public enum ShapeSet
    {
        Set = 0,
        Union = 1,
        Intersect = 2,
        Subtract = 3,
        Invert = 4,
        Null = 5
    }

    [LibraryImport("libX11", StringMarshalling = StringMarshalling.Utf8)]
    public static partial IntPtr XOpenDisplay(string display);

    [LibraryImport("libX11")]
    public static partial int XCloseDisplay(IntPtr display);

    [DllImport("libXext")]
    public static extern void XShapeCombineRegion(IntPtr display, IntPtr window, Shape destKind,
        int xOff, int yOff, IntPtr region, ShapeSet op);

    public static void SetLaunch(bool start)
    {
        Task.Run(() =>
        {
            try
            {
                // if (!File.Exists("/etc/systemd/user/colordesktop.service"))
                // {
                //     {
                //         var assm = Assembly.GetExecutingAssembly();
                //         using var item = assm.GetManifestResourceStream("ColorDesktop.Launcher.Resource.linux.service")!;
                //         using var file = File.Open(Program.RunDir + "colordesktop.service", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                //         item.CopyTo(file);
                //     }
                //     Run("pkexec","cp " + Program.RunDir + "colordesktop.service" + " /etc/systemd/user/colordesktop.service");
                //     Run("systemctl", "--user daemon-reload");
                // }

                // if (start)
                // {
                //     Run("systemctl", "--user enable colordesktop.service");
                // }
                // else
                // {
                //     Run("systemctl", "--user disable colordesktop.service");
                // }

                if (start)
                {
                    if (!File.Exists("/etc/xdg/autostart/colordesktop.desktop"))
                    {
                        var assm = Assembly.GetExecutingAssembly();
                        using var item = assm.GetManifestResourceStream("ColorDesktop.Launcher.Resource.linux.desktop")!;
                        using var file = File.Open(Program.RunDir + "colordesktop.desktop", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        item.CopyTo(file);
                    }

                    Run("pkexec", "cp " + Program.RunDir + "colordesktop.desktop" + " /etc/xdg/autostart/colordesktop.desktop");
                }
                else
                {
                    if (File.Exists("/etc/xdg/autostart/colordesktop.desktop"))
                    {
                        Run("pkexec", "rm /etc/xdg/autostart/colordesktop.desktop");
                    }
                }
            }
            catch (Exception e)
            {

            }
        });
    }

    private static void Run(string pg, string cmd)
    {
        var p = Process.Start(pg, cmd);
        p.WaitForExit();
    }

    public static void SetMouseThrough(Window window, bool enable)
    {
        if (window.TryGetPlatformHandle() is { } platformHandle)
        {
            IntPtr display = XOpenDisplay(null!);

            // 定义鼠标穿透的形状
            if (enable)
            {
                // 设置鼠标穿透
                XShapeCombineRegion(display, platformHandle.Handle, Shape.Input, 0, 0, IntPtr.Zero, ShapeSet.Null);
            }
            else
            {
                // 恢复正常鼠标交互（默认不穿透）
                XShapeCombineRegion(display, platformHandle.Handle, Shape.Input, 0, 0, IntPtr.Zero, ShapeSet.Set);
            }

            XCloseDisplay(display);
        }
    }
}