using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace ColorDesktop.AnalogClockPlugin;

public partial class FlipControl : UserControl
{
    public FlipControl()
    {
        InitializeComponent();

        DataContextChanged += FlipControl_DataContextChanged;
    }

    private void FlipControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is FlipModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private async void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(FlipModel.Image1))
        {
            var model = (DataContext as FlipModel)!;
            Image1.Source = model.Image;
            (ImageCov.RenderTransform as Rotate3DTransform)!.AngleX = 0;
            ImageCov.IsVisible = true;
            await Task.Run(() =>
            {
                int value = 0;
                do
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        (ImageCov.RenderTransform as Rotate3DTransform)!.AngleX = value;
                    });
                    value -= 10;
                    Thread.Sleep(20);
                }
                while (value > -90);
            });
            ImageCov.IsVisible = false;
            Image3.Source = model.Image;
            (ImageCov.RenderTransform as Rotate3DTransform)!.AngleX = 0;
            Image4.Source = model.Image1;
            (ImageCov1.RenderTransform as Rotate3DTransform)!.AngleX = 90;
            ImageCov1.IsVisible = true;
            await Task.Run(() =>
            {
                int value = 90;
                do
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        (ImageCov1.RenderTransform as Rotate3DTransform)!.AngleX = value;
                    });
                    value -= 10;
                    Thread.Sleep(20);
                }
                while (value > 0);
            });
            ImageCov1.IsVisible = false;
            (ImageCov1.RenderTransform as Rotate3DTransform)!.AngleX = 0;
            Image2.Source = model.Image1;
        }
        else if (e.PropertyName == nameof(FlipModel.Clear))
        {
            Image1.Source = null;
            Image2.Source = null;
            Image3.Source = null;
            Image4.Source = null;
        }
    }
}