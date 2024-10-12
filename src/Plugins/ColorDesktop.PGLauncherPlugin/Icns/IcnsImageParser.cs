using SkiaSharp;

namespace ColorDesktop.PGLauncherPlugin.Icns;

public static class IcnsImageParser
{
    public static int ICNS_MAGIC = IcnsType.TypeAsInt("icns");

    private static int Read4Bytes(Stream stream)
    {
        byte byte0 = (byte)stream.ReadByte();
        byte byte1 = (byte)stream.ReadByte();
        byte byte2 = (byte)stream.ReadByte();
        byte byte3 = (byte)stream.ReadByte();

        return ((0xff & byte0) << 24) |
               ((0xff & byte1) << 16) |
               ((0xff & byte2) << 8) |
               ((0xff & byte3) << 0);
    }

    private static void Write4Bytes(Stream stream, int value)
    {
        stream.WriteByte((byte)((value & 0xff000000) >> 24));
        stream.WriteByte((byte)((value & 0x00ff0000) >> 16));
        stream.WriteByte((byte)((value & 0x0000ff00) >> 8));
        stream.WriteByte((byte)((value & 0x000000ff) >> 0));
    }

    private class IcnsHeader(int magic, int fileSize)
    {
        public int magic = magic; // Magic literal (4 bytes), always "icns"
        public int fileSize = fileSize; // Length of file (4 bytes), in bytes.
    }

    private static IcnsHeader ReadIcnsHeader(Stream stream)
    {
        int Magic = Read4Bytes(stream);
        int FileSize = Read4Bytes(stream);

        if (Magic != ICNS_MAGIC)
            throw new Exception("Wrong ICNS magic");

        return new IcnsHeader(Magic, FileSize);
    }

    public class IcnsElement(int type, int elementSize, byte[] data)
    {
        public int type = type;
        public int elementSize = elementSize;
        public byte[] data = data;
    }

    private static IcnsElement ReadIcnsElement(Stream stream)
    {
        int type = Read4Bytes(stream); // Icon type (4 bytes)
        int elementSize = Read4Bytes(stream); // Length of data (4 bytes), in bytes, including this header
        byte[] data = new byte[elementSize - 8];
        stream.Read(data, 0, data.Length);

        return new IcnsElement(type, elementSize, data);
    }

    private static IcnsElement[] ReadImage(Stream stream)
    {
        var icnsHeader = ReadIcnsHeader(stream);

        var icnsElementList = new List<IcnsElement>();
        for (int remainingSize = icnsHeader.fileSize - 8;
             remainingSize > 0;)
        {
            var icnsElement = ReadIcnsElement(stream);
            icnsElementList.Add(icnsElement);
            remainingSize -= icnsElement.elementSize;
        }

        var icnsElements = new IcnsElement[icnsElementList.Count];
        for (int i = 0; i < icnsElements.Length; i++)
            icnsElements[i] = icnsElementList[i];

        return icnsElements;
    }

    public static IcnsImage? GetImage(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open);
        return GetImage(stream);
    }

    public static IcnsImage? GetImage(Stream stream)
    {
        var icnsContents = ReadImage(stream);
        var result = IcnsDecoder.DecodeAllImages(icnsContents);
        if (result.Count <= 0)
            throw new NotSupportedException("No icons in ICNS file");

        IcnsImage? max = null;
        foreach (var bitmap in result)
            if (bitmap.Bitmap != null && (max == null || (bitmap.Bitmap.Width > bitmap.Bitmap.Height)))
                max = bitmap;

        return max;
    }

    public static List<IcnsImage> GetImages(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open);
        return GetImages(stream);
    }

    public static List<IcnsImage> GetImages(Stream stream)
    {
        var icnsContents = ReadImage(stream);
        return IcnsDecoder.DecodeAllImages(icnsContents);
    }

    public static void WriteImage(SKBitmap src, Stream stream)
    {
        var imageType = IcnsType.FindType(src.Width, src.Height, 32, IcnsType.TypeDetails.None)
            ?? throw new NotSupportedException($"Invalid/unsupported source: {src.Width}x{src.Height}");
        Write4Bytes(stream, ICNS_MAGIC);
        Write4Bytes(stream, 4 + 4 + 4 + 4 + 4 * imageType.Width * imageType.Height + 4 + 4 + imageType.Width * imageType.Height);

        Write4Bytes(stream, imageType.Type);
        Write4Bytes(stream, 4 + 4 + 4 * imageType.Width * imageType.Height);

        // the image
        for (int y = 0; y < src.Height; y++)
        {
            for (int x = 0; x < src.Width; x++)
            {
                var argb = src.GetPixel(x, y);
                stream.WriteByte(0);
                stream.WriteByte(argb.Red);
                stream.WriteByte(argb.Green);
                stream.WriteByte(argb.Blue);
            }
        }

        // mask
        var maskType = IcnsType.FindType(src.Width, src.Height, 8, IcnsType.TypeDetails.Mask)!;
        Write4Bytes(stream, maskType.Type);
        Write4Bytes(stream, 4 + 4 + imageType.Width * imageType.Width);

        for (int y = 0; y < src.Height; y++)
        {
            for (int x = 0; x < src.Width; x++)
            {
                var argb = src.GetPixel(x, y);
                stream.WriteByte(argb.Alpha);
            }
        }
    }

    public static void WriteImage(SKBitmap src, string filename)
    {
        using var stream = new FileStream(filename, FileMode.Create);
        WriteImage(src, stream);
    }
}