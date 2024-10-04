namespace ColorDesktop.PGLauncherPlugin.Icns;

internal class Rle24Compression
{
    public static byte[] Decompress(int width, int height, byte[] data)
    {
        int pixelCount = width * height;
        byte[] result = new byte[4 * pixelCount];

        // Several ICNS parsers advance by 4 bytes here:
        // http://code.google.com/p/icns2png/ - when the width is >= 128
        // http://icns.sourceforge.net/ - when those 4 bytes are all zero
        //
        // A scan of all .icns files on MacOS shows that
        // all 128x128 images indeed start with 4 zeroes,
        // while all smaller images don't.
        // However it is dangerous to assume
        // that 4 initial zeroes always need to be skipped,
        // because they could encode valid pixels on smaller images.
        // So always skip on 128x128, and never skip on anything else.
        int dataPos = 0;
        if (width >= 128 && height >= 128)
            dataPos = 4;

        // argb, band by band in 3 passes, with no alpha
        for (int band = 1; band <= 3; band++)
        {
            int remaining = pixelCount;
            int resultPos = 0;
            while (remaining > 0)
            {
                if ((data[dataPos] & 0x80) != 0)
                {
                    int count = (0xff & data[dataPos]) - 125;
                    for (int i = 0; i < count; i++)
                    {
                        int idx = band + 4 * (resultPos++);
                        if (idx < result.Length)
                            result[idx] = data[dataPos + 1];
                    }
                    dataPos += 2;
                    remaining -= count;
                }
                else
                {
                    int count = (0xff & data[dataPos]) + 1;
                    dataPos++;
                    for (int i = 0; i < count; i++)
                    {
                        byte value = data[dataPos++];
                        int idx = band + 4 * (resultPos++);
                        if (idx < result.Length)
                            result[idx] = value;
                    }
                    remaining -= count;
                }
            }
        }
        return result;
    }
}
