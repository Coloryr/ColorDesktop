using System;
using System.Text;

namespace ColorDesktop.PGLauncherPlugin.Icns;

public class IcnsType
{
    private readonly int type;
    private readonly int width;
    private readonly int height;
    private readonly int bitsPerPixel;
    private readonly TypeDetails details;

    // https://en.wikipedia.org/wiki/Apple_Icon_Image_format
    public static readonly IcnsType[] ALL_TYPES =
    [
        // 16x12
        new("icm#", 16, 12, 1, TypeDetails.HasMask),
        new("icm4", 16, 12, 4, TypeDetails.None),
        new("icm8", 16, 12, 8, TypeDetails.None),
        // 16x16
        new("ics#", 16, 16, 1, TypeDetails.Mask),
        new("ics4", 16, 16, 4, TypeDetails.None),
        new("ics8", 16, 16, 8, TypeDetails.None),
        new("is32", 16, 16, 32, TypeDetails.None),
        new("s8mk", 16, 16, 8, TypeDetails.Mask),
        new("icp4", 16, 16, 32, TypeDetails.Compressed),
        new("ic04", 16, 16, 32, TypeDetails.ARGB),
        // 18x18
        new("icsb", 18, 18, 32, TypeDetails.ARGB), // not tested
        // 32x32
        new("ICON", 32, 32, 1, TypeDetails.None),
        new("ICN#", 32, 32, 1, TypeDetails.HasMask),
        new("icl4", 32, 32, 4, TypeDetails.None),
        new("icl8", 32, 32, 8, TypeDetails.None),
        new("il32", 32, 32, 32, TypeDetails.None),
        new("l8mk", 32, 32, 8, TypeDetails.Mask),
        new("icp5", 32, 32, 32, TypeDetails.Compressed),
        new("ic11", 32, 32, 32, TypeDetails.Retina),
        new("ic05", 32, 32, 32, TypeDetails.ARGB),
        // 36x36      
        new("icsB", 36, 36, 32, TypeDetails.ARGB), // not tested
        // 48x48
        new("ich#", 48, 48, 1, TypeDetails.Mask),
        new("ich4", 48, 48, 4, TypeDetails.None),
        new("ich8", 48, 48, 8, TypeDetails.None),
        new("ih32", 48, 48, 32, TypeDetails.None),
        new("h8mk", 48, 48, 8, TypeDetails.Mask),
        // 64x64      
        new("icp6", 64, 64, 32, TypeDetails.Compressed),
        new("ic12", 64, 64, 32, TypeDetails.Retina),
        // 128x128
        new("it32", 128, 128, 32, TypeDetails.None),
        new("t8mk", 128, 128, 8, TypeDetails.Mask),
        new("ic07", 128, 128, 32, TypeDetails.Compressed),
        // other
        new("ic08", 256, 256, 32, TypeDetails.Compressed),
        new("ic13", 256, 256, 32, TypeDetails.Retina),
        new("ic09", 512, 512, 32, TypeDetails.Compressed),
        new("ic14", 512, 512, 32, TypeDetails.Retina),
        new("ic10", 1024, 1024, 32, TypeDetails.Retina),
    ];

    private IcnsType(string type, int width, int height, int bitsPerPixel, TypeDetails details)
    {
        this.type = TypeAsInt(type);
        this.width = width;
        this.height = height;
        this.bitsPerPixel = bitsPerPixel;
        this.details = details;
    }

    public int Type
    {
        get { return type; }
    }

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    public int BitsPerPixel
    {
        get { return bitsPerPixel; }
    }

    public TypeDetails Details
    {
        get { return details; }
    }

    public static IcnsType? FindType(int type, TypeDetails ignoreDetails)
    {
        for (int i = 0; i < ALL_TYPES.Length; i++)
        {
            if (ALL_TYPES[i].type != type)
                continue;

            if (ignoreDetails != 0 && ALL_TYPES[i].Details == ignoreDetails)
                continue;

            return ALL_TYPES[i];
        }
        return null;
    }

    public static IcnsType? FindType(int width, int height, int bpp, TypeDetails details)
    {
        for (int i = 0; i < ALL_TYPES.Length; i++)
        {
            var type = ALL_TYPES[i];
            if (type.width == width &&
                type.height == height &&
                type.bitsPerPixel == bpp &&
                type.details == details)
                return type;
        }

        return null;
    }

    public static int TypeAsInt(string type)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(type);

        if (bytes.Length != 4)
            throw new Exception("Invalid ICNS type");

        return ((0xff & bytes[0]) << 24) |
               ((0xff & bytes[1]) << 16) |
               ((0xff & bytes[2]) << 8) |
               (0xff & bytes[3]);
    }

    public static string DescribeType(int type)
    {
        byte[] bytes =
        [
            (byte)(0xff & (type >> 24)),
            (byte)(0xff & (type >> 16)),
            (byte)(0xff & (type >> 8)),
            (byte)(0xff & type),
        ];
        return Encoding.ASCII.GetString(bytes);
    }

    public enum TypeDetails
    {
        /// <summary>
        /// The default image with no detils.
        /// </summary>
        None,
        /// <summary>
        /// The image is alpha mask.
        /// </summary>
        Mask,
        /// <summary>
        /// Has alpha mask.
        /// </summary>
        HasMask,
        /// <summary>
        /// Whole 4 channels are used.
        /// </summary>
        ARGB,
        /// <summary>
        /// Compressed, j2k or PNG codec is used.
        /// </summary>
        Compressed,
        /// <summary>
        /// Retina (2x) image. j2k or PNG is used.
        /// </summary>
        Retina,
    }
}