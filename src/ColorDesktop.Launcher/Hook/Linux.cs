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
    public enum Shape : int
    {
        Bounding = 0,
        Clip = 1,
        Input = 2
    }

    public enum ShapeSet : int
    {
        Set = 0,
        Union = 1,
        Intersect = 2,
        Subtract = 3,
        Invert = 4,
        Null = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XSetWindowAttributes
    {
        public int background_pixmap;
        public ulong background_pixel;
        public int border_pixmap;
        public ulong border_pixel;
        public int bit_gravity;
        public int win_gravity;
        public int backing_store;
        public ulong backing_planes;
        public ulong backing_pixel;
        public bool save_under;
        public ulong event_mask;
        public ulong do_not_propagate_mask;
        public bool override_redirect;
        public IntPtr colormap;
        public IntPtr cursor;
    }

    // 定义X11所需的外部函数和常量
    private const string X11Lib = "libX11.so";
    private const string XextLib = "libXext.so";

    [DllImport(XextLib)]
    private static extern IntPtr XCreateRegion();

    [DllImport(XextLib)]
    private static extern void XDestroyRegion(IntPtr region);

    [DllImport(XextLib)]
    private static extern void XShapeCombineRegion(IntPtr display, IntPtr window, Shape destKind,
        int xOff, int yOff, IntPtr region, ShapeSet op);

    [DllImport(XextLib)]
    private static extern void XShapeCombineMask(IntPtr display, IntPtr window, Shape destKind,
        int xOff, int yOff, IntPtr region, ShapeSet op);

    [DllImport(XextLib)]
    private static extern int XUnionRectWithRegion(ref XRectangle rectangle, IntPtr src_region, IntPtr dest_region_return);

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

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct XRectangle
    {
        public short x;
        public short y;
        public short width;
        public short height;
    }

    public static void SetMouseThrough(Window window, bool enable)
    {
        if (window.PlatformImpl is { } platformHandle
            && window.TryGetPlatformHandle() is { } handel)
        {
            Type x11WindowType = platformHandle.GetType();

            var x11FieldInfo = x11WindowType.GetField("_x11", BindingFlags.NonPublic | BindingFlags.Instance);
            if (x11FieldInfo == null)
            {
                return;
            }

            var x11FieldValue = x11FieldInfo.GetValue(platformHandle);
            if (x11FieldValue == null)
            {
                return;
            }

            Type x11InfoType = x11FieldValue.GetType();

            var displayPropertyInfo = x11InfoType.GetProperty("Display", BindingFlags.Public | BindingFlags.Instance);
            if (displayPropertyInfo == null)
            {
                return;
            }

            var display = (IntPtr)displayPropertyInfo.GetValue(x11FieldValue)!;

            if (enable)
            {
                var region = XCreateRegion();
                XShapeCombineRegion(display, handel.Handle, Shape.Input, 0, 0, region, ShapeSet.Set);
                XDestroyRegion(region);
            }
            else
            {
                XShapeCombineMask(display, handel.Handle, Shape.Input, 0, 0, IntPtr.Zero, ShapeSet.Set);
            }
        }
    }
}