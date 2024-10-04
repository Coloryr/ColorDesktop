using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using ColorDesktop.Api;
using ColorDesktop.PGLauncherPlugin.Icns;
using IniParser;
using IniParser.Model;
using Microsoft.VisualBasic;

namespace ColorDesktop.PGLauncherPlugin;

public static class SystemUtils
{
    public static Bitmap? GetIcon(string local)
    {
        switch (SystemInfo.Os)
        {
            case OsType.Windows:
                if (!local.EndsWith(".exe"))
                {
                    return null;
                }
                return Win32IconUtils.GetIconFromExe(local);
            case OsType.Linux:
                if (local.EndsWith(".desktop"))
                {
                    var parser = new FileIniDataParser();
                    IniData data = parser.ReadFile(local);
                    var data1 = data["Desktop Entry"];
                    if (data1.ContainsKey("Icon"))
                    {
                        var icon = data1["Icon"];
                        if (File.Exists(icon))
                        {
                            return new(icon);
                        }

                        var icon1 = "/usr/share/icons/" + icon;
                        if (File.Exists(icon1))
                        {
                            return new(icon1);
                        }

                        icon1 = "/usr/share/icons/" + icon + ".png";
                        if (File.Exists(icon1))
                        {
                            return new(icon1);
                        }
                    }

                    return null;
                }
                break;
            case OsType.MacOS:
                if (local.EndsWith(".app"))
                {
                    var document = new XmlDocument();
                    document.Load(local + "/Contents/Info.plist");
                    var node1 = document.GetElementsByTagName("dict")[0]!;
                    bool findicon = false;
                    string icon = "";
                    foreach (XmlNode item in node1)
                    {
                        if (findicon)
                        {
                            icon = item.InnerText;
                            break;
                        }
                        if (item.InnerText == "CFBundleIconFile")
                        {
                            findicon = true;
                        }
                    }
                    if (findicon)
                    {
                        var img = IcnsImageParser.GetImage(local + "/Contents/Resources/" + icon);
                        if (img != null)
                        {
                            var bitmap = img.Bitmap;
                            var img1 = new WriteableBitmap(new(bitmap.Width, bitmap.Height), new(96, 96));
                            unsafe
                            {
                                using var temp = img1.Lock();
                                var size = bitmap.Width * bitmap.Height * 4;
                                Buffer.MemoryCopy((void*)bitmap.GetPixels(), (void*)temp.Address, size, size);
                            }
                            return img1;
                        }
                    }
                }
                break;
        }

        return null;
    }

    /// <summary>
    /// 打开文件
    /// </summary>
    /// <param name="top">窗口</param>
    /// <param name="title">标题</param>
    /// <param name="ext">后缀</param>
    /// <param name="name">名字</param>
    /// <param name="multiple">多选</param>
    /// <param name="storage">首选路径</param>
    /// <returns></returns>
    public static async Task<IReadOnlyList<IStorageFile>?> SelectFile(TopLevel? top, string title,
        string[]? ext, string name, bool multiple = false, DirectoryInfo? storage = null)
    {
        if (top == null)
            return null;

        var defaultFolder = storage == null ? null : await top.StorageProvider.TryGetFolderFromPathAsync(storage.FullName);

        return await top.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = title,
            AllowMultiple = multiple,
            SuggestedStartLocation = defaultFolder,
            FileTypeFilter = ext == null ? null : new List<FilePickerFileType>()
            {
                new(name)
                {
                     Patterns = new List<string>(ext)
                }
            }
        });
    }

    /// <summary>
    /// 文件转字符串
    /// </summary>
    /// <param name="file">文件</param>
    /// <returns>路径字符串</returns>
    public static string? GetPath(this IStorageFile file)
    {
        if (SystemInfo.Os == OsType.Android)
        {
            return file.Path.AbsoluteUri;
        }
        return file.Path.LocalPath;
    }

    public static bool IsExecutable(string file)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            return false;
        }
        if (SystemInfo.Os == OsType.MacOS)
        {
            if (!Directory.Exists(file) && !File.Exists(file))
            {
                return false;
            }
        }
        else
        {
            if (!File.Exists(file))
            {
                return false;
            }
        }

        return true;
    }

    public static void Launch(PGItemObj obj)
    {
        if (SystemInfo.Os == OsType.Windows)
        {
            if (!File.Exists(obj.Local))
            {
                return;
            }
            Process.Start(obj.Local, obj.Arg);
        }
        else if (SystemInfo.Os == OsType.MacOS)
        {
            if (Directory.Exists(obj.Local))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "open", // macOS的命令行工具，用于打开文件和应用程序
                    Arguments = $"\"{obj.Local}\" {obj.Arg}", // 使用引号包裹路径并添加参数
                    UseShellExecute = false // 使用系统外壳程序执行
                });
            }
            else if (File.Exists(obj.Local))
            {
                Process.Start(obj.Local, obj.Arg);
            }
        }
        else
        {
            if (obj.Local.EndsWith(".desktop"))
            {
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(obj.Local);
                var data1 = data["Desktop Entry"];
                if (data1.ContainsKey("Exec"))
                {
                    Process.Start(data1["Exec"], obj.Arg);
                }
            }
            else
            {
                if (!File.Exists(obj.Local))
                {
                    return;
                }
                Process.Start(obj.Local, obj.Arg);
            }
        }
    }
}

public static class Win32IconUtils
{
    [DllImport("shell32.dll")]
    public static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern void DeleteDC(IntPtr hdc);

    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

    [DllImport("gdi32.dll")]
    public static extern bool GetObject(IntPtr hObject, int nCount, out BITMAP lpObject);

    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern bool GetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines,
        IntPtr lpvBits, ref BITMAPINFO lpbmi, uint uColorUse);

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }

    // 定义必要的结构和常量
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public IntPtr bmBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    private const uint IMAGE_ICON = 1;
    private const uint LR_COPYFROMRESOURCE = 0x00000002;

    public static Bitmap? GetIconFromExe(string exePath)
    {
        int count = ExtractIconEx(exePath, -1, null!, null!, 0);

        if (count < 1)
        {
            return null;
        }

        IntPtr[] largeIcons = new IntPtr[count];
        ExtractIconEx(exePath, 0, largeIcons, null!, count);
        var img = HIconToSKBitmap(largeIcons[0]);

        foreach (var item in largeIcons)
        {
            DestroyIcon(item);
        }

        return img;
    }

    public static Bitmap HIconToSKBitmap(IntPtr hIcon)
    {
        // Get icon dimensions
        GetIconInfo(hIcon, out var iconInfo);

        // Create a compatible DC
        IntPtr hdc = CreateCompatibleDC(IntPtr.Zero);
        IntPtr hBitmap = iconInfo.hbmColor;
        GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out BITMAP bmp);

        var bih = new BITMAPINFO()
        {
            biSize = 40,
            biWidth = bmp.bmWidth,
            biHeight = bmp.bmHeight,
            biPlanes = 1,
            biBitCount = 32, // 使用32位颜色
            biCompression = 0, //BI_RGB
            biSizeImage = 0, // 可以设为0
            biXPelsPerMeter = 0,
            biYPelsPerMeter = 0,
            biClrUsed = 0,
            biClrImportant = 0
        };

        var ptr1 = Marshal.AllocHGlobal(bmp.bmWidth * bmp.bmHeight * 4);
        var ptr2 = Marshal.AllocHGlobal(bmp.bmWidth * bmp.bmHeight * 4);
        GetDIBits(hdc, hBitmap, 0, (uint)bmp.bmHeight, ptr1, ref bih, 0); // DIB_RGB_COLORS
        // 每行的字节数
        int rowSize = bmp.bmWidth * 4; // 每个像素4字节

        for (int y = 0; y < bmp.bmHeight; y++)
        {
            // 拷贝当前行到目标行
            unsafe
            {
                Buffer.MemoryCopy((void*)(ptr1 + ((bmp.bmHeight - 1 - y) * rowSize)), (void*)(ptr2 + (y * rowSize)), rowSize, rowSize);
            }
        }

        Marshal.FreeHGlobal(ptr1);

        var skbitmap = new Bitmap(PixelFormat.Bgra8888, AlphaFormat.Unpremul, ptr2, new PixelSize(bmp.bmWidth, bmp.bmHeight), new Vector(96, 96), rowSize);

        Marshal.FreeHGlobal(ptr2);

        DeleteObject(iconInfo.hbmMask);
        DeleteObject(iconInfo.hbmColor);
        DeleteDC(hdc);

        // Convert SKBitmap to SKImage
        return skbitmap;
    }
}
