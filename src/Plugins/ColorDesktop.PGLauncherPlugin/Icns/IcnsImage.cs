using System;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace ColorDesktop.PGLauncherPlugin.Icns;

public class IcnsImage
{
    private readonly SKBitmap bitmap;
    private readonly IcnsType type;

    public IcnsImage(SKBitmap bitmap, IcnsType type)
    {
        this.bitmap = bitmap;
        this.type = type;
    }

    public SKBitmap Bitmap
    {
        get { return bitmap; }
    }

    public IcnsType Type
    {
        get { return type; }
    }
}
