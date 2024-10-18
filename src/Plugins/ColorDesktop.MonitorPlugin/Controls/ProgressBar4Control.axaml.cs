using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ColorDesktop.MonitorPlugin.Controls;

public partial class ProgressBar4Control : UserControl
{
    public ProgressBar4Control()
    {
        InitializeComponent();
    }
}

public class Circular1ProgressBar : Control
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<CircularProgressBar, double>(nameof(Value));

    public static readonly StyledProperty<IBrush> BackgroundColorProperty =
        AvaloniaProperty.Register<CircularProgressBar, IBrush>(nameof(BackgroundColor), Brushes.Gray);

    public static readonly StyledProperty<IBrush> ForegroundColorProperty =
        AvaloniaProperty.Register<CircularProgressBar, IBrush>(nameof(ForegroundColor), Brushes.Blue);

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public IBrush BackgroundColor
    {
        get => GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public IBrush ForegroundColor
    {
        get => GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty)
        {
            InvalidateVisual();
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        // 定义圆环的参数
        double centerX = Bounds.Width / 2;
        double centerY = Bounds.Height / 2;
        double radius = Math.Min(Bounds.Width, Bounds.Height) / 2 - 5;

        var pen = new Pen(ForegroundColor, 5);
        var pen1 = new Pen(BackgroundColor, 10);

        context.DrawGeometry(null, pen1, Convert(centerX, centerY, 100, radius));

        context.DrawGeometry(null, pen, Convert(centerX, centerY, Value, radius));
    }

    private double DegreesToRadians(double degrees) => degrees * Math.PI / 180;

    public Geometry Convert(double centerX, double centerY, double value, double radius)
    {
        double PIch2 = 2 * Math.PI;

        double angle = (double)value / 100 * PIch2;

        var x1 = Math.Sin(0);
        var y1 = Math.Cos(0);

        var x2 = Math.Sin(angle);
        var y2 = Math.Cos(angle);

        var p1 = new Point(centerX + radius * x1, centerY - radius * y1);
        var p2 = new Point(centerX + radius * x2, centerY - radius * y2);

        string data;

        if (angle == 0)
        {
            data = string.Empty;
        }
        else if (angle > 0 && angle < Math.PI)
        {
            data = $"M{p1.X},{p1.Y} A{radius},{radius},0,0,1 {p2.X},{p2.Y}";
        }
        else if (angle >= Math.PI && angle < 2 * Math.PI)
        {
            data = $"M{p1.X},{p1.Y} A{radius},{radius},0,1,1 {p2.X},{p2.Y}";
        }
        else
        {
            data = $"M{p1.X},{p1.Y} A{radius},{radius},0,1,1 {p2.X - 0.001},{p2.Y} Z";
        }

        return Geometry.Parse(data);
    }
}