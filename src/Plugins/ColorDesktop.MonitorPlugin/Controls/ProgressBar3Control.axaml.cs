using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ColorDesktop.MonitorPlugin.Controls;

public partial class ProgressBar3Control : UserControl
{
    public ProgressBar3Control()
    {
        InitializeComponent();
    }
}

public class CircularProgressBar : Control
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<CircularProgressBar, double>(nameof(Value));

    public static readonly StyledProperty<IBrush> BackgroundColorProperty =
       AvaloniaProperty.Register<CircularProgressBar, IBrush>(nameof(BackgroundColor), Brushes.Gray);

    public static readonly StyledProperty<IBrush> ForegroundColorProperty =
        AvaloniaProperty.Register<CircularProgressBar, IBrush>(nameof(ForegroundColor), Brushes.White);

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
        double dx = 0;
        double dy = 0;
        var size = Math.Min(Bounds.Width, Bounds.Height);
        if (Bounds.Width > Bounds.Height)
        {
            dx = (Bounds.Width - Bounds.Height) / 2;
        }
        else if (Bounds.Height > Bounds.Width)
        {
            dy = (Bounds.Height - Bounds.Width) / 2;
        }
        var radius = size / 2;
        var center = new Point(dx + size / 2, dy + size / 2);

        context.FillRectangle(BackgroundColor, new Rect(dx, dy, size, size), (float)size);

        var angle = 360 * (Value / 100);
        var startAngle = -90;
        var endAngle = startAngle + angle;

        var startPoint = new Point(center.X + radius * Math.Cos(DegreesToRadians(startAngle)),
                                    center.Y + radius * Math.Sin(DegreesToRadians(startAngle)));
        var endPoint = new Point(center.X + radius * Math.Cos(DegreesToRadians(endAngle)),
                                  center.Y + radius * Math.Sin(DegreesToRadians(endAngle)));

        var pathGeometry = new StreamGeometry();

        using (var ctx = pathGeometry.Open())
        {
            ctx.BeginFigure(center, true);
            ctx.LineTo(startPoint, true);
            ctx.ArcTo(endPoint, new Size(radius, radius), 0, angle > 180, SweepDirection.Clockwise, true);
            ctx.LineTo(center, true);
        }

        context.DrawGeometry(ForegroundColor, new Pen(), pathGeometry);
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
}