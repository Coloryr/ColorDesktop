using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ColorDesktop.PGLauncherPlugin.Icns
{
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

    private class IcnsHeader
    {
      public int magic; // Magic literal (4 bytes), always "icns"
      public int fileSize; // Length of file (4 bytes), in bytes.

      public IcnsHeader(int magic, int fileSize)
      {
        this.magic = magic;
        this.fileSize = fileSize;
      }
    }

    private static IcnsHeader ReadIcnsHeader(Stream stream)
    {
      int Magic = Read4Bytes(stream);
      int FileSize = Read4Bytes(stream);

      if (Magic != ICNS_MAGIC)
        throw new Exception("Wrong ICNS magic");

      return new IcnsHeader(Magic, FileSize);
    }

    public class IcnsElement
    {
      public int type;
      public int elementSize;
      public byte[] data;

      public IcnsElement(int type, int elementSize, byte[] data)
      {
        this.type = type;
        this.elementSize = elementSize;
        this.data = data;
      }
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
      IcnsHeader icnsHeader = ReadIcnsHeader(stream);

      List<IcnsElement> icnsElementList = new List<IcnsElement>();
      for (int remainingSize = icnsHeader.fileSize - 8;
           remainingSize > 0; )
      {
        IcnsElement icnsElement = ReadIcnsElement(stream);
        icnsElementList.Add(icnsElement);
        remainingSize -= icnsElement.elementSize;
      }

      IcnsElement[] icnsElements = new IcnsElement[icnsElementList.Count];
      for (int i = 0; i < icnsElements.Length; i++)
        icnsElements[i] = icnsElementList[i];

      return icnsElements;
    }

    public static IcnsImage GetImage(string filename)
    {
      using (FileStream stream = new FileStream(filename, FileMode.Open))
        return GetImage(stream);
    }

    public static IcnsImage GetImage(Stream stream)
    {
      IcnsElement[] icnsContents = ReadImage(stream);
      List<IcnsImage> result = IcnsDecoder.DecodeAllImages(icnsContents);
      if (result.Count <= 0)
        throw new NotSupportedException("No icons in ICNS file");

      IcnsImage max = null;
      foreach (IcnsImage bitmap in result)
        if (bitmap.Bitmap != null && (max == null || (bitmap.Bitmap.Width > bitmap.Bitmap.Height)))
          max = bitmap;

      return max;
    }

    public static List<IcnsImage> GetImages(string filename)
    {
      using (FileStream stream = new FileStream(filename, FileMode.Open))
        return GetImages(stream);
    }

    public static List<IcnsImage> GetImages(Stream stream)
    {
      IcnsElement[] icnsContents = ReadImage(stream);
      return IcnsDecoder.DecodeAllImages(icnsContents);
    }

    public static void WriteImage(Bitmap src, Stream stream)
    {
      IcnsType imageType = IcnsType.FindType(src.Width, src.Height, 32, IcnsType.TypeDetails.None);
      if (imageType == null)
        throw new NotSupportedException($"Invalid/unsupported source: {src.Width}x{src.Height}");

      Write4Bytes(stream, ICNS_MAGIC);
      Write4Bytes(stream, 4 + 4 + 4 + 4 + 4*imageType.Width*imageType.Height + 4 + 4 + imageType.Width*imageType.Height);

      Write4Bytes(stream, imageType.Type);
      Write4Bytes(stream, 4 + 4 + 4 * imageType.Width * imageType.Height);

      BitmapData bitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

      // the image
      for (int y = 0; y < src.Height; y++)
      {
        for (int x = 0; x < src.Width; x++)
        {
          uint argb = IcnsDecoder.GetPixel(bitmapData, x, y);
          stream.WriteByte(0);
          stream.WriteByte((byte)((argb & 0x00ff0000) >> 16));
          stream.WriteByte((byte)((argb & 0x0000ff00) >> 8));
          stream.WriteByte((byte)((argb & 0x000000ff) >> 0));
        }
      }

      // mask
      IcnsType maskType = IcnsType.FindType(src.Width, src.Height, 8, IcnsType.TypeDetails.Mask);
      Write4Bytes(stream, maskType.Type);
      Write4Bytes(stream, 4 + 4 + imageType.Width * imageType.Width);

      for (int y = 0; y < src.Height; y++)
      {
        for (int x = 0; x < src.Width; x++)
        {
          uint argb = IcnsDecoder.GetPixel(bitmapData, x, y);
          stream.WriteByte((byte)((argb & 0xff000000) >> 24));
        }
      }
    }

    public static void WriteImage(Bitmap src, string filename)
    {
      using (FileStream stream = new FileStream(filename, FileMode.Create))
        WriteImage(src, stream);
    }
  }

}