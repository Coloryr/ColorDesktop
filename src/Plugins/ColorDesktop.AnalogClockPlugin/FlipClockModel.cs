using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorDesktop.ClockPlugin;
using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;

namespace ColorDesktop.AnalogClockPlugin;

public partial class FlipClockModel : ObservableObject
{
    private readonly Bitmap[,] _numbers = new Bitmap[10, 2];

    public FlipModel HourA { get; init; } = new();
    public FlipModel HourB { get; init; } = new();
    public FlipModel MinuteA { get; init; } = new();
    public FlipModel MinuteB { get; init; } = new();
    public FlipModel SecondA { get; init; } = new();
    public FlipModel SecondB { get; init; } = new();

    public void Tick()
    {
        var time = ClockPlugin.ClockPlugin.Config.Ntp ? NtpClient.Date : DateTime.Now;

        HourA.SetImage(_numbers[time.Hour / 10, 0], _numbers[time.Hour / 10, 1]);
        HourB.SetImage(_numbers[time.Hour % 10, 0], _numbers[time.Hour % 10, 1]);
        MinuteA.SetImage(_numbers[time.Minute / 10, 0], _numbers[time.Minute / 10, 1]);
        MinuteB.SetImage(_numbers[time.Minute % 10, 0], _numbers[time.Minute % 10, 1]);
        SecondA.SetImage(_numbers[time.Second / 10, 0], _numbers[time.Second / 10, 1]);
        SecondB.SetImage(_numbers[time.Second % 10, 0], _numbers[time.Second % 10, 1]);
    }

    public void Update(AnalogClockInstanceConfigObj obj)
    {
        HourA.Size = obj.Size;
        HourB.Size = obj.Size;
        MinuteA.Size = obj.Size;
        MinuteB.Size = obj.Size;
        SecondA.Size = obj.Size;
        SecondB.Size = obj.Size;

        HourA.Clear();
        HourB.Clear();
        MinuteA.Clear();
        MinuteB.Clear();
        SecondA.Clear();
        SecondB.Clear();

        foreach (var item in _numbers)
        {
            item?.Dispose();
        }

        SKTypeface font;
        if (!string.IsNullOrWhiteSpace(obj.Font)
            && FontManager.Current.SystemFonts.Any(a => a.Name == obj.Font)
            && SKFontManager.Default.MatchFamily(obj.Font) is { } font1)
        {
            font = font1;
        }
        else
        {
            font = SKFontManager.Default.MatchFamily(FontFamily.DefaultFontFamilyName);
        }
        var font2 = new SKFont(font, obj.TextSize);
        var paint = new SKPaint(font2)
        {
            Color = SKColor.Parse(obj.TextColor),
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };

        var color = Brush.Parse(obj.BackColor);
        HourA.Color = color;
        HourB.Color = color;
        MinuteA.Color = color;
        MinuteB.Color = color;
        SecondA.Color = color;
        SecondB.Color = color;
        var color1 = Brush.Parse(obj.BorderColor);
        HourA.Color1 = color1;
        HourB.Color1 = color1;
        MinuteA.Color1 = color1;
        MinuteB.Color1 = color1;
        SecondA.Color1 = color1;
        SecondB.Color1 = color1;

        for (int i = 0; i < 10; i++)
        {
            var textBounds = new SKRect();
            paint.MeasureText($"{i}", ref textBounds);

            int width = obj.Size;
            int height = obj.Size * 2;
            using var image = new SKBitmap(width, height);
            using var draw = new SKCanvas(image);

            float x = (width - textBounds.Width) / 2; // 水平居中
            float y = (height - textBounds.Height) / 2 + textBounds.Height; // 垂直居中

            draw.DrawText($"{i}", obj.Size / 2, y, paint);

            using var image1 = new SKBitmap(width, width);
            using var draw1 = new SKCanvas(image1);
            draw1.DrawBitmap(image, new SKRect(0, 0, obj.Size, obj.Size), new SKRect(0, 0, obj.Size, obj.Size));

            using var image2 = new SKBitmap(width, width);
            using var draw2 = new SKCanvas(image2);
            draw2.DrawBitmap(image, new SKRect(0, obj.Size, obj.Size, obj.Size * 2), new SKRect(0, 0, obj.Size, obj.Size));

            var bitmap1 = new Bitmap(PixelFormat.Rgba8888, AlphaFormat.Unpremul,
                   image1.GetPixels(), new(width, width), new(96, 96), image1.RowBytes);
            _numbers[i, 0] = bitmap1;

            var bitmap2 = new Bitmap(PixelFormat.Rgba8888, AlphaFormat.Unpremul,
                image2.GetPixels(), new(width, width), new(96, 96), image2.RowBytes);
            _numbers[i, 1] = bitmap2;
        }
    }
}
