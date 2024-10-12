using SkiaSharp;

namespace ColorDesktop.PGLauncherPlugin.Icns;

public static class IcnsDecoder
{
    private static readonly uint[] PALETE_4BPP =
    [
      0xffffffff,
      0xfffcf305,
      0xffff6402,
      0xffdd0806,
      0xfff20884,
      0xff4600a5,
      0xff0000d4,
      0xff02abea,
      0xff1fb714,
      0xff006411,
      0xff562c05,
      0xff90713a,
      0xffc0c0c0,
      0xff808080,
      0xff404040,
      0xff000000
    ];

    private static readonly uint[] PALETTE_8BPP =
    [
      0xFFFFFFFF,
      0xFFFFFFCC,
      0xFFFFFF99,
      0xFFFFFF66,
      0xFFFFFF33,
      0xFFFFFF00,
      0xFFFFCCFF,
      0xFFFFCCCC,
      0xFFFFCC99,
      0xFFFFCC66,
      0xFFFFCC33,
      0xFFFFCC00,
      0xFFFF99FF,
      0xFFFF99CC,
      0xFFFF9999,
      0xFFFF9966,
      0xFFFF9933,
      0xFFFF9900,
      0xFFFF66FF,
      0xFFFF66CC,
      0xFFFF6699,
      0xFFFF6666,
      0xFFFF6633,
      0xFFFF6600,
      0xFFFF33FF,
      0xFFFF33CC,
      0xFFFF3399,
      0xFFFF3366,
      0xFFFF3333,
      0xFFFF3300,
      0xFFFF00FF,
      0xFFFF00CC,
      0xFFFF0099,
      0xFFFF0066,
      0xFFFF0033,
      0xFFFF0000,
      0xFFCCFFFF,
      0xFFCCFFCC,
      0xFFCCFF99,
      0xFFCCFF66,
      0xFFCCFF33,
      0xFFCCFF00,
      0xFFCCCCFF,
      0xFFCCCCCC,
      0xFFCCCC99,
      0xFFCCCC66,
      0xFFCCCC33,
      0xFFCCCC00,
      0xFFCC99FF,
      0xFFCC99CC,
      0xFFCC9999,
      0xFFCC9966,
      0xFFCC9933,
      0xFFCC9900,
      0xFFCC66FF,
      0xFFCC66CC,
      0xFFCC6699,
      0xFFCC6666,
      0xFFCC6633,
      0xFFCC6600,
      0xFFCC33FF,
      0xFFCC33CC,
      0xFFCC3399,
      0xFFCC3366,
      0xFFCC3333,
      0xFFCC3300,
      0xFFCC00FF,
      0xFFCC00CC,
      0xFFCC0099,
      0xFFCC0066,
      0xFFCC0033,
      0xFFCC0000,
      0xFF99FFFF,
      0xFF99FFCC,
      0xFF99FF99,
      0xFF99FF66,
      0xFF99FF33,
      0xFF99FF00,
      0xFF99CCFF,
      0xFF99CCCC,
      0xFF99CC99,
      0xFF99CC66,
      0xFF99CC33,
      0xFF99CC00,
      0xFF9999FF,
      0xFF9999CC,
      0xFF999999,
      0xFF999966,
      0xFF999933,
      0xFF999900,
      0xFF9966FF,
      0xFF9966CC,
      0xFF996699,
      0xFF996666,
      0xFF996633,
      0xFF996600,
      0xFF9933FF,
      0xFF9933CC,
      0xFF993399,
      0xFF993366,
      0xFF993333,
      0xFF993300,
      0xFF9900FF,
      0xFF9900CC,
      0xFF990099,
      0xFF990066,
      0xFF990033,
      0xFF990000,
      0xFF66FFFF,
      0xFF66FFCC,
      0xFF66FF99,
      0xFF66FF66,
      0xFF66FF33,
      0xFF66FF00,
      0xFF66CCFF,
      0xFF66CCCC,
      0xFF66CC99,
      0xFF66CC66,
      0xFF66CC33,
      0xFF66CC00,
      0xFF6699FF,
      0xFF6699CC,
      0xFF669999,
      0xFF669966,
      0xFF669933,
      0xFF669900,
      0xFF6666FF,
      0xFF6666CC,
      0xFF666699,
      0xFF666666,
      0xFF666633,
      0xFF666600,
      0xFF6633FF,
      0xFF6633CC,
      0xFF663399,
      0xFF663366,
      0xFF663333,
      0xFF663300,
      0xFF6600FF,
      0xFF6600CC,
      0xFF660099,
      0xFF660066,
      0xFF660033,
      0xFF660000,
      0xFF33FFFF,
      0xFF33FFCC,
      0xFF33FF99,
      0xFF33FF66,
      0xFF33FF33,
      0xFF33FF00,
      0xFF33CCFF,
      0xFF33CCCC,
      0xFF33CC99,
      0xFF33CC66,
      0xFF33CC33,
      0xFF33CC00,
      0xFF3399FF,
      0xFF3399CC,
      0xFF339999,
      0xFF339966,
      0xFF339933,
      0xFF339900,
      0xFF3366FF,
      0xFF3366CC,
      0xFF336699,
      0xFF336666,
      0xFF336633,
      0xFF336600,
      0xFF3333FF,
      0xFF3333CC,
      0xFF333399,
      0xFF333366,
      0xFF333333,
      0xFF333300,
      0xFF3300FF,
      0xFF3300CC,
      0xFF330099,
      0xFF330066,
      0xFF330033,
      0xFF330000,
      0xFF00FFFF,
      0xFF00FFCC,
      0xFF00FF99,
      0xFF00FF66,
      0xFF00FF33,
      0xFF00FF00,
      0xFF00CCFF,
      0xFF00CCCC,
      0xFF00CC99,
      0xFF00CC66,
      0xFF00CC33,
      0xFF00CC00,
      0xFF0099FF,
      0xFF0099CC,
      0xFF009999,
      0xFF009966,
      0xFF009933,
      0xFF009900,
      0xFF0066FF,
      0xFF0066CC,
      0xFF006699,
      0xFF006666,
      0xFF006633,
      0xFF006600,
      0xFF0033FF,
      0xFF0033CC,
      0xFF003399,
      0xFF003366,
      0xFF003333,
      0xFF003300,
      0xFF0000FF,
      0xFF0000CC,
      0xFF000099,
      0xFF000066,
      0xFF000033,
      0xFFEE0000,
      0xFFDD0000,
      0xFFBB0000,
      0xFFAA0000,
      0xFF880000,
      0xFF770000,
      0xFF550000,
      0xFF440000,
      0xFF220000,
      0xFF110000,
      0xFF00EE00,
      0xFF00DD00,
      0xFF00BB00,
      0xFF00AA00,
      0xFF008800,
      0xFF007700,
      0xFF005500,
      0xFF004400,
      0xFF002200,
      0xFF001100,
      0xFF0000EE,
      0xFF0000DD,
      0xFF0000BB,
      0xFF0000AA,
      0xFF000088,
      0xFF000077,
      0xFF000055,
      0xFF000044,
      0xFF000022,
      0xFF000011,
      0xFFEEEEEE,
      0xFFDDDDDD,
      0xFFBBBBBB,
      0xFFAAAAAA,
      0xFF888888,
      0xFF777777,
      0xFF555555,
      0xFF444444,
      0xFF222222,
      0xFF111111,
      0xFF000000
    ];

    // http://www.libpng.org/pub/png/spec/1.2/PNG-Structure.html
    private static readonly byte[] PNG_SIGNATURE = [137, 80, 78, 71, 13, 10, 26, 10];

    private static IcnsImage? TryDecodingPng(IcnsImageParser.IcnsElement element, IcnsType imageType)
    {
        if (element.data.Length < PNG_SIGNATURE.Length)
            return null; // definitely not a valid png

        for (int i = 0; i < PNG_SIGNATURE.Length; i++)
            if (element.data[i] != PNG_SIGNATURE[i])
                return null; // not a png

        using var ms = new MemoryStream(element.data);
        return new IcnsImage(SKBitmap.Decode(ms), imageType); // cast is valid, for PNG it will be Bitmap
    }

    private static void Decode1BPPImage(IcnsType imageType, byte[] imageData, SKBitmap image)
    {
        int position = 0;
        int bitsLeft = 0;
        int value = 0;
        for (int y = 0; y < imageType.Height; y++)
        {
            for (int x = 0; x < imageType.Width; x++)
            {
                if (bitsLeft == 0)
                {
                    value = 0xff & imageData[position++];
                    bitsLeft = 8;
                }

                uint argb;
                argb = (value & 0x80u) != 0 ? 0xff000000u : 0xffffffffu;
                value <<= 1;
                bitsLeft--;

                image.SetPixel(x, y, argb);
            }
        }
    }

    private static void Decode4BPPImage(IcnsType imageType, byte[] imageData, SKBitmap image)
    {
        int i = 0;
        bool visited = false;
        for (int y = 0; y < imageType.Height; y++)
            for (int x = 0; x < imageType.Width; x++)
            {
                int index;
                if (!visited)
                    index = 0xf & (imageData[i] >> 4);
                else
                    index = 0xf & imageData[i++];
                visited = !visited;

                image.SetPixel(x, y, PALETE_4BPP[index]);
            }
    }

    private static void Decode8BPPImage(IcnsType imageType, byte[] imageData, SKBitmap image)
    {
        for (int y = 0; y < imageType.Height; y++)
            for (int x = 0; x < imageType.Width; x++)
            {
                int index = 0xff & imageData[y * imageType.Width + x];

                image.SetPixel(x, y, PALETTE_8BPP[index]);
            }
    }

    private static void Decode32BPPImage(IcnsType imageType, byte[] imageData, SKBitmap image)
    {
        for (int y = 0; y < imageType.Height; y++)
            for (int x = 0; x < imageType.Width; x++)
            {
                uint argb = (0xff000000u /* the "alpha" is ignored */|
                             ((0xffu & imageData[4 * (y * imageType.Width + x) + 1]) << 16) |
                             ((0xffu & imageData[4 * (y * imageType.Width + x) + 2]) << 8) |
                             (0xffu & imageData[4 * (y * imageType.Width + x) + 3]));

                image.SetPixel(x, y, argb);
            }
    }

    private static void Decode32BPPImageARGB(IcnsType imageType, byte[] imageData, SKBitmap image)
    {
        for (int y = 0; y < imageType.Height; y++)
            for (int x = 0; x < imageType.Width; x++)
            {
                uint argb = (((0xffu & imageData[4 * (y * imageType.Width + x) + 0]) << 24) |
                             ((0xffu & imageData[4 * (y * imageType.Width + x) + 1]) << 16) |
                             ((0xffu & imageData[4 * (y * imageType.Width + x) + 2]) << 8) |
                             (0xffu & imageData[4 * (y * imageType.Width + x) + 3]));

                image.SetPixel(x, y, argb);
            }
    }

    private static void Apply1BPPMask(byte[] maskData, SKBitmap image)
    {
        int position;
        int bitsLeft = 0;
        int value = 0;

        // 1 bit icon types have image data followed by mask data in the same entry
        int totalBytes = (image.Width * image.Height + 7) / 8;

        if (maskData.Length >= 2 * totalBytes)
            position = totalBytes;
        else
            throw new ArgumentException("1 BPP mask underrun parsing ICNS file");

        for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
            {
                if (bitsLeft == 0)
                {
                    value = 0xff & maskData[position++];
                    bitsLeft = 8;
                }

                uint alpha;
                alpha = (value & 0x80u) != 0 ? 0xffu : 0x00u;
                value <<= 1;
                bitsLeft--;

                image.SetPixel(x, y, (alpha << 24) | (0xffffffu & (uint)image.GetPixel(x, y)));
            }
    }

    private static void Apply8BPPMask(byte[] maskData, SKBitmap image)
    {
        for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
            {
                uint alpha = 0xffu & maskData[y * image.Width + x];
                image.SetPixel(x, y, alpha << 24 | (0xffffffu & (uint)image.GetPixel(x, y)));
            }
    }

    private static IcnsImageParser.IcnsElement? FindElement(IEnumerable<IcnsImageParser.IcnsElement> elements, int targetType)
    {
        foreach (IcnsImageParser.IcnsElement element in elements)
            if (element.type == targetType)
                return element;

        return null;
    }

    private static IcnsImage? DecodeImage(IcnsImageParser.IcnsElement imageElement, IcnsImageParser.IcnsElement[] icnsElements)
    {
        var imageType = IcnsType.FindType(imageElement.type, IcnsType.TypeDetails.Mask);
        if (imageType == null)
            return null;

        IcnsType? maskType = null;
        IcnsImageParser.IcnsElement? maskElement = null;

        if (imageType.Details == IcnsType.TypeDetails.HasMask)
        {
            maskType = imageType;
            maskElement = imageElement;
        }
        else if (imageType.Details == IcnsType.TypeDetails.None)
        {
            maskType = IcnsType.FindType(imageType.Width, imageType.Height, 8, IcnsType.TypeDetails.Mask);
            if (maskType != null)
                maskElement = FindElement(icnsElements, maskType.Type);

            if (maskElement == null)
            {
                maskType = IcnsType.FindType(imageType.Width, imageType.Height, 1, IcnsType.TypeDetails.Mask);
                if (maskType != null)
                    maskElement = FindElement(icnsElements, maskType.Type);
            }
        }

        if (imageType.Details == IcnsType.TypeDetails.Compressed ||
            imageType.Details == IcnsType.TypeDetails.Retina)
        {
            var result = TryDecodingPng(imageElement, imageType);
            if (result != null)
                return result; // png
        }

        int expectedSize = (imageType.Width * imageType.Height * imageType.BitsPerPixel + 7) / 8;
        byte[] imageData;

        if (imageElement.data.Length < expectedSize)
        {
            if (imageType.BitsPerPixel == 32)
                imageData = Rle24Compression.Decompress(imageType.Width, imageType.Height, imageElement.data);
            else
                throw new Exception("Short image data but not a 32 bit compressed type");
        }
        else
            imageData = imageElement.data;

        var image = new SKBitmap(imageType.Width, imageType.Height);

        switch (imageType.BitsPerPixel)
        {
            case 1:
                Decode1BPPImage(imageType, imageData, image);
                break;

            case 4:
                Decode4BPPImage(imageType, imageData, image);
                break;

            case 8:
                Decode8BPPImage(imageType, imageData, image);
                break;

            case 32:
                if (imageType.Details == IcnsType.TypeDetails.ARGB)
                    Decode32BPPImageARGB(imageType, imageData, image);
                else
                    Decode32BPPImage(imageType, imageData, image);
                break;

            default:
                image.Dispose();
                throw new NotSupportedException("Unsupported bit depth " + imageType.BitsPerPixel);
        }

        if (maskElement != null)
        {
            switch (maskType?.BitsPerPixel)
            {
                case 1:
                    Apply1BPPMask(maskElement.data, image);
                    break;
                case 8:
                    Apply8BPPMask(maskElement.data, image);
                    break;
                default:
                    image.Dispose();
                    throw new NotSupportedException("Unsupport mask bit depth " + maskType?.BitsPerPixel);
            }
        }

        return new IcnsImage(image, imageType);
    }

    public static List<IcnsImage> DecodeAllImages(IcnsImageParser.IcnsElement[] icnsElements)
    {
        var result = new List<IcnsImage>();

        for (int i = 0; i < icnsElements.Length; i++)
        {
            var image = DecodeImage(icnsElements[i], icnsElements);
            if (image != null)
                result.Add(image);
        }
        return result;
    }
}